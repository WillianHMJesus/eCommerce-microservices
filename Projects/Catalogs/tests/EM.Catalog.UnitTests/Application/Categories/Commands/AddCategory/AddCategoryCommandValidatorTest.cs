using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidatorTest
{
    private readonly IFixture _fixture;

    public AddCategoryCommandValidatorTest() => _fixture = new Fixture();

    [Theory, AutoCategoryData]
    public async Task Constructor_ValidAddCategoryCommand_ShouldReturnValidResult(
        [Frozen] Mock<IReadRepository> repositoryMock,
        AddCategoryCommandValidator sut,
        AddCategoryCommand command)
    {
        repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        ValidationResult result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_ZeroAddCategoryCommandCode_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryCodeLessThanEqualToZero);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyAddCategoryCommandName_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NullAddCategoryCommandName_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryNameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_EmptyAddCategoryCommandDescription_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryDescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_NullAddCategoryCommandDescription_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        AddCategoryCommand command = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryDescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    public async Task Constructor_DuplicityAddCategoryCommand_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut,
        AddCategoryCommand command)
    {
        IEnumerable<Category> categories = new List<Category>() { _fixture.Create<Category>() };

        var result = await sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.CategoryRegisterDuplicity);
    }
}
