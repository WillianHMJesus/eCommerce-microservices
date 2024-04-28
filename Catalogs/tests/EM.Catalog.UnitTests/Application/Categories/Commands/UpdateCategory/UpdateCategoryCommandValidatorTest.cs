using AutoFixture;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Domain;
using FluentValidation.Results;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidatorTest
{
    private readonly Fixture _fixture;
    private readonly UpdateCategoryCommandValidator _validator;

    public UpdateCategoryCommandValidatorTest()
    {
        _fixture = new();
        _validator = new();
    }

    [Fact]
    public void Constructor_ValidUpdateCategoryCommand_ShouldReturnValidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Create<UpdateCategoryCommand>();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Constructor_InvalidUpdateCategoryCommandId_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryInvalidId);
    }

    [Fact]
    public void Constructor_ZeroUpdateCategoryCommandCode_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryCodeLessThanEqualToZero);
    }

    [Fact]
    public void Constructor_EmptyUpdateCategoryCommandName_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullUpdateCategoryCommandName_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_EmptyUpdateCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullUpdateCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = _validator.Validate(updateCategoryCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }
}
