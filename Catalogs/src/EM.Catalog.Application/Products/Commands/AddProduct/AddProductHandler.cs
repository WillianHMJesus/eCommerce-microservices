using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductHandler : ICommandHandler<AddProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IMapper _mapper;

    public AddProductHandler(
        IWriteRepository writeRepository,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        Category? category = await _writeRepository.GetCategoryByIdAsync(command.CategoryId, cancellationToken);

        if (category == null)
            return Result.CreateResponseWithErrors("CategoryId", ErrorMessage.ProductCategoryNotFound);

        Product product = _mapper.Map<Product>(command);
        product.AssignCategory(category);

        await _writeRepository.AddProductAsync(product, cancellationToken);

        return Result.CreateResponseWithData(product.Id);
    }
}
