using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    private readonly IReadRepository _repository;

    public AddCategoryCommandValidator(IReadRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(Key.CategoryCodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CategoryNameNullOrEmpty);

        RuleFor(x => x.Description)
           .Must(x => !string.IsNullOrEmpty(x))
           .WithMessage(Key.CategoryDescriptionNullOrEmpty);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(Key.CategoryRegisterDuplicity);
    }

    public async Task<bool> ValidateDuplicityAsync(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories = await _repository.GetCategoriesByCodeOrName(command.Code, command.Name, cancellationToken);

        return !categories.Any();
    }
}
