using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
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
        [Frozen] Mock<IReadRepository> repositoryMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

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
    public async Task Constructor_UpdateCategoryCommandIdNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IReadRepository> repositoryMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        Category? category = null;
        repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNotFound);
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
    public async Task Constructor_DuplicityUpdateCategoryCommand_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        IEnumerable<Category> categories = new List<Category>() { _fixture.Create<Category>() };

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryRegisterDuplicity);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NotDuplicityUpdateCategoryCommand_ShouldReturnValidResult(
        [Frozen] Mock<IReadRepository> repositoryMock,
        UpdateCategoryCommandValidator sut)
    {
        List<Category> categories = new List<Category> { _fixture.Create<Category>() };
        repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        UpdateCategoryCommand command = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Id, categories[0].Id)
            .Create();

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
