using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidatorTest
{
    private readonly IFixture _fixture;

    public UpdateCategoryCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCategoryData]
    public async Task Constructor_ValidUpdateCategoryCommand_ShouldReturnValidResult(
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_InvalidUpdateCategoryCommandId_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryInvalidId);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_ZeroUpdateCategoryCommandCode_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryCodeLessThanEqualToZero);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyUpdateCategoryCommandName_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NullUpdateCategoryCommandName_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyUpdateCategoryCommandDescription_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryDescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NullUpdateCategoryCommandDescription_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryDescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_UpdateCategoryCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<ICategoryValidations> validationsMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNotFound);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_DuplicityUpdateCategoryCommand_ShouldReturnInvalidResult(
        [Frozen] Mock<ICategoryValidations> validationsMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        validationsMock
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryRegisterDuplicity);
    }
}
