using AutoFixture;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Domain;
using FluentValidation.Results;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidatorTest
{
    private readonly Fixture _fixture;
    private readonly UpdateProductCommandValidator _validator;

    public UpdateProductCommandValidatorTest()
    {
        _fixture = new();
        _validator = new();
    }

    [Fact]
    public void Constructor_ValidUpdateProductCommand_ShouldReturnValidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Create<UpdateProductCommand>();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Constructor_EmptyUpdateProductCommandId_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductInvalidId);
    }

    [Fact]
    public void Constructor_EmptyUpdateProductCommandName_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullUpdateProductCommandName_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Constructor_EmptyUpdateProductCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullUpdateProductCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Constructor_ZeroUpdateProductCommandValue_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Value, 0)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public void Constructor_ZeroUpdateProductCommandQuantity_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Quantity, 0)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductQuantityLessThanEqualToZero);
    }

    [Fact]
    public void Constructor_EmptyUpdateProductCommandImage_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Image, "")
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Constructor_NullUpdateProductCommandImage_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.Image, null as string)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Constructor_EmptyUpdateProductCommandCategoryId_ShouldReturnInvalidResult()
    {
        UpdateProductCommand updateProductCommand = _fixture.Build<UpdateProductCommand>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        ValidationResult result = _validator.Validate(updateProductCommand);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.ErrorMessage == ErrorMessage.ProductInvalidCategoryId);
    }
}
