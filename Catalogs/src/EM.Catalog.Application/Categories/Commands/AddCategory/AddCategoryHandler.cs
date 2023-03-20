using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public class AddCategoryHandler : ICommandHandler<AddCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public AddCategoryHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<Result> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = new(command.Code, command.Name, command.Description);
        await _categoryRepository.AddCategoryAsync(category);

        return Result.CreateResponseWithData(category.Id);
    }
}
