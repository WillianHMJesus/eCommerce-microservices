﻿using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IReadRepository _repository;

    public UpdateCategoryCommandValidator(IReadRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .GreaterThan(Guid.Empty)
            .WithMessage(ErrorMessage.CategoryInvalidId);

        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(ErrorMessage.CategoryCodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(ErrorMessage.CategoryNameNullOrEmpty);

        RuleFor(x => x.Description)
           .Must(x => !string.IsNullOrEmpty(x))
           .WithMessage(ErrorMessage.CategoryDescriptionNullOrEmpty);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(ErrorMessage.CategoryRegisterDuplicity);
    }

    public async Task<bool> ValidateDuplicityAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories = await _repository.GetCategoriesByCodeOrName(command.Code, command.Name, cancellationToken);

        return !categories.Any(x => x.Id != command.Id); 
    }
}