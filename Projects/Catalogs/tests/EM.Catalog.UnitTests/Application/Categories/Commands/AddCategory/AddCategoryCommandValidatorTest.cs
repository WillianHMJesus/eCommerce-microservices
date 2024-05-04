using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidatorTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly AddCategoryCommandValidator _validator;
    private readonly AddCategoryCommand _addCategoryCommand;

    public AddCategoryCommandValidatorTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = _fixture.Freeze<Mock<IReadRepository>>();
        _validator = _fixture.Create<AddCategoryCommandValidator>();
        _addCategoryCommand = _fixture.Create<AddCategoryCommand>();
    }

    [Fact]
    public async Task Constructor_ValidAddCategoryCommand_ShouldReturnValidResult()
    {
        ValidationResult result = await _validator.ValidateAsync(_addCategoryCommand);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Constructor_ZeroAddCategoryCommandCode_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        var result = await _validator.ValidateAsync(addCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryCodeLessThanEqualToZero);
    }

    [Fact]
    public async Task Constructor_EmptyAddCategoryCommandName_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        var result = await _validator.ValidateAsync(addCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullAddCategoryCommandName_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        var result = await _validator.ValidateAsync(addCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_EmptyAddCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        var result = await _validator.ValidateAsync(addCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullAddCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        var result = await _validator.ValidateAsync(addCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_DuplicityAddCategoryCommand_ShouldReturnInvalidResult()
    {
        IEnumerable<Category> categories = new List<Category>() { _fixture.Create<Category>() };

        _repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(categories));

        var result = await _validator.ValidateAsync(_addCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryRegisterDuplicity);
    }
}
