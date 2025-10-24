using EM.Catalog.Application.Abstractions;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.DeleteProduct;
using EM.Catalog.Application.Products.Commands.InactivateProduct;
using EM.Catalog.Application.Products.Commands.ReactivateProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductDeleted;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain;
using WH.SharedKernel;
using WH.SharedKernel.Abstractions;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Products.Commands;

public sealed class ProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediator mediator) :
    CommandBase(unitOfWork, mediator),
    ICommandHandler<AddProductCommand>,
    ICommandHandler<DeleteProductCommand>,
    ICommandHandler<ReactivateProductCommand>,
    ICommandHandler<InactivateProductCommand>,
    ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        Product product = mapper.Map<AddProductCommand, Product>(command);
        await repository.AddAsync(product, cancellationToken);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => mapper.Map<Product, ProductAddedEvent>(product),
            Product.ErrorSavingProduct,
            Result.CreateResponseWithData(product.Id)
        );
    }

    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        Product product = await GetProductByIdAsync(command.Id, cancellationToken);
        repository.Delete(product);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => new ProductDeletedEvent() { Id = product.Id },
            Product.ErrorSavingProduct
        );
    }

    public async Task<Result> Handle(ReactivateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = await GetProductByIdAsync(command.Id, cancellationToken);

        if (product.Available)
        {
            return Result.CreateResponseWithData();
        }

        product.Reactivate();
        repository.UpdateAvailability(product);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => mapper.Map<Product, ProductAddedEvent>(product),
            Product.ErrorSavingProduct
        );
    }

    public async Task<Result> Handle(InactivateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = await GetProductByIdAsync(command.Id, cancellationToken);

        if (!product.Available)
        {
            return Result.CreateResponseWithData();
        }

        product.Inactivate();
        repository.UpdateAvailability(product);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => new ProductDeletedEvent() { Id = product.Id },
            Product.ErrorSavingProduct
        );
    }

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = mapper.Map<UpdateProductCommand, Product>(command);
        repository.Update(product);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => mapper.Map<Product, ProductUpdatedEvent>(product),
            Product.ErrorSavingProduct
        );
    }

    private async Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(productId, cancellationToken);
        DomainException.ThrowIfNull(product, Product.ProductNotFound);

        return product;
    }
}
