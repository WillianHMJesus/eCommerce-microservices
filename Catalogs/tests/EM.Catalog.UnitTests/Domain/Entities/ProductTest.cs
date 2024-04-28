using AutoFixture;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Shared.Core;
using Xunit;

namespace EM.Catalog.UnitTests.Domain.Entities;

public sealed class ProductTest
{
    private readonly Fixture _fixture;

    public ProductTest() => _fixture = new();

    [Fact]
    public void Validate_ValidProduct_ShouldNotReturnDomainException()
    {
        Product product = _fixture.Create<Product>();

        Exception domainException = Record.Exception(() => product.Validate());

        Assert.Null(domainException);
    }

    [Fact]
    public void Validate_EmptyProductName_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Name, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_NullProductName_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Name, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_EmptyProductDescription_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Description, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductDescriptionNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_NullProductDescription_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Description, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductDescriptionNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_ZeroProductValue_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Value, 0)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductValueLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void Validate_EmptyProductImage_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Image, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductImageNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_NullProductImage_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Image, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductImageNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void MakeAvailable_InvalidProduct_ShouldMakeAvailableProduct()
    {
        Product product = _fixture.Create<Product>();

        product.MakeAvailable();

        Assert.True(product.Available);
    }

    [Fact]
    public void MakeUnavailable_ValidProduct_ShouldMakeUnavailableProduct()
    {
        Product product = _fixture.Create<Product>();

        product.MakeUnavailable();

        Assert.False(product.Available);
    }

    [Fact]
    public void AddQuantity_ValidQuantity_ShouldAddProductQuantity()
    {
        Product product = _fixture.Create<Product>();
        short quantityAdded = _fixture.Create<short>();

        product.AddQuantity(quantityAdded);

        Assert.Equal(quantityAdded, product.Quantity);
    }

    [Fact]
    public void AddQuantity_QuantityAddZero_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();

        DomainException domainException = Assert.Throws<DomainException>(() => product.AddQuantity(0));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductQuantityAddedLessThanOrEqualToZero, domainException.Message);
    }

    [Fact]
    public void RemoveQuantity_ValidQuantity_ShouldRemoveProductQuantity()
    {
        Product product = _fixture.Create<Product>();
        product.AddQuantity(_fixture.Create<short>());

        product.RemoveQuantity(product.Quantity);

        Assert.Equal(0, product.Quantity);
    }

    [Fact]
    public void RemoveQuantity_QuantityDebitedZero_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();

        DomainException domainException = Assert.Throws<DomainException>(() => product.RemoveQuantity(0));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductQuantityDebitedLessThanOrEqualToZero, domainException.Message);
    }

    [Fact]
    public void RemoveQuantity_ProductQuantityZero_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();
        short debitQuantity = (short)(product.Quantity + 1);

        DomainException domainException = Assert.Throws<DomainException>(() => product.RemoveQuantity(debitQuantity));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductQuantityDebitedLargerThanAvailable, domainException.Message);
    }

    [Fact]
    public void AddCategory_ValidCategory_ShouldAddProductCategory()
    {
        Product product = _fixture.Create<Product>();

        product.AssignCategory(_fixture.Create<Category>());

        Assert.NotNull(product.Category);
    }

    [Fact]
    public void AddCategory_NullValue_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();

#pragma warning disable CS8625
        DomainException domainException = Assert.Throws<DomainException>(() => product.AssignCategory(null));
#pragma warning restore CS8625

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductCategoryNull, domainException.Message);
    }
}
