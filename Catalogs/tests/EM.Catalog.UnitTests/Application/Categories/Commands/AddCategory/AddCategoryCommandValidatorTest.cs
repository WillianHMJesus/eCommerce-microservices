using AutoFixture;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Domain;
using FluentValidation.Results;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidatorTest
{
    private readonly Fixture _fixture;
    private readonly AddCategoryCommandValidator _validator;

    public AddCategoryCommandValidatorTest()
    {
        _fixture = new();
        _validator = new();
    }

    [Fact]
    public void Constructor_ValidAddCategoryCommand_ShouldReturnValidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Create<AddCategoryCommand>();

        ValidationResult result = _validator.Validate(addCategoryCommand);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Constructor_ZeroAddCategoryCommandCode_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        var result = _validator.Validate(addCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryCodeLessThanEqualToZero);
    }

    [Fact]
    public void Constructor_EmptyAddCategoryCommandName_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        var result = _validator.Validate(addCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullAddCategoryCommandName_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        var result = _validator.Validate(addCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_EmptyAddCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        var result = _validator.Validate(addCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullAddCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        AddCategoryCommand addCategoryCommand = _fixture.Build<AddCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        var result = _validator.Validate(addCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }
}
