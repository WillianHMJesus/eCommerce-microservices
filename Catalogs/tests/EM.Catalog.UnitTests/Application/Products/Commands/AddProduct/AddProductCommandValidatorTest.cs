using AutoFixture;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Domain;
using FluentValidation.Results;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidatorTest
{
    private readonly Fixture _fixture;
    private readonly AddProductCommandValidator _validator;

    public AddProductCommandValidatorTest()
    {
        _fixture = new();
        _validator = new();
    }

    [Fact]
    public void Constructor_ValidAddProductCommand_ShouldReturnValidResult()
    {
        AddProductCommand addProductCommand = _fixture.Create<AddProductCommand>();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Constructor_EmptyAddProductCommandName_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullAddProductCommandName_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_EmptyAddProductCommandDescription_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullAddProductCommandDescription_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Constructor_ZeroAddProductCommandValue_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Value, 0)
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public void Constructor_ZeroAddProductCommandQuantity_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductQuantityLessThanEqualToZero);
    }

    [Fact]
    public void Constructor_EmptyAddProductCommandImage_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Image, "")
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullAddProductCommandImage_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.Image, null as string)
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Constructor_EmptyAddProductCommandCategoryId_ShouldReturnInvalidResult()
    {
        AddProductCommand addProductCommand = _fixture.Build<AddProductCommand>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        ValidationResult result = _validator.Validate(addProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductInvalidCategoryId);
    }
}
