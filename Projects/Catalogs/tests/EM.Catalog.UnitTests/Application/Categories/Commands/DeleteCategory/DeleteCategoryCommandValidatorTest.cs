using AutoFixture.Xunit2;
using AutoFixture;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using FluentValidation.Results;
using FluentAssertions;
using EM.Common.Core.ResourceManagers;
using EM.Catalog.Application.Categories.Validations;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidatorTest
{
    private readonly IFixture _fixture;

    public DeleteCategoryCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCategoryData]
    public async Task Constructor_ValidUpdateCategoryCommand_ShouldReturnValidResult(
        DeleteCategoryCommandValidator sut,
        DeleteCategoryCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyUpdateCategoryCommandId_ShouldReturnInvalidResult(
        DeleteCategoryCommandValidator sut)
    {
        DeleteCategoryCommand command = _fixture.Build<DeleteCategoryCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryInvalidId);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_UpdateCategoryCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<ICategoryValidations> validationsMock,
        DeleteCategoryCommandValidator sut,
        DeleteCategoryCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNotFound);
    }
}
