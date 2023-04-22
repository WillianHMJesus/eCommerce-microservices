using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryHandler : ICommandHandler<AddCategoryCommand>
{
    private readonly IProductRepository _productRepository;

    public AddCategoryHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Result> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = new(command.Code, command.Name, command.Description);
        await _productRepository.AddCategoryAsync(category, cancellationToken);

        return Result.CreateResponseWithData(category.Id);
    }
}
