using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.Fixtures;
using Xunit;

namespace EM.Catalog.UnitTests.Domain;

public class ProductTest
{
    private readonly ProductFixture _productFixture;

    public ProductTest()
    {
        _productFixture = new ProductFixture();
    }

    [Fact]
    public void Validate_ValidProduct_MustNotReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _productFixture.GenerateProductWithInvalidName());

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidProductName_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _productFixture.GenerateProductWithInvalidName());

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidProductDescription_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _productFixture.GenerateProductWithInvalidDescription());

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageDescriptionNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidProductValue_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _productFixture.GenerateProductWithInvalidValue());

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageValueLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidProductQuantity_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _productFixture.GenerateProductWithInvalidQuantity());

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageQuantityLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidProductImage_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _productFixture.GenerateProductWithInvalidImage());

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageImageNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Enable_InvalidProduct_MustEnableProduct()
    {
        Product product = _productFixture.GenerateProduct();
        product.Disable();

        product.Enable();

        Assert.True(product.Active);
    }

    [Fact]
    public void Disable_ValidProduct_MustDisableProduct()
    {
        Product product = _productFixture.GenerateProduct();

        product.Disable();

        Assert.False(product.Active);
    }

    [Fact]
    public void DebitQuantity_ValidQuantity_MustDebitProductQuantity()
    {
        Product product = _productFixture.GenerateProduct();

        product.DebitQuantity(product.Quantity);

        Assert.Equal(0, product.Quantity);
    }

    [Fact]
    public void DebitQuantity_QuantityDebitedZero_MustReturnDomainException()
    {
        Product product = _productFixture.GenerateProduct();

        DomainException domainException = Assert.Throws<DomainException>(() => product.DebitQuantity(0));

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageQuantityDebitedLessThanOrEqualToZero, domainException.Message);
    }

    [Fact]
    public void DebitQuantity_ProductQuantityZero_MustReturnDomainException()
    {
        Product product = _productFixture.GenerateProduct();
        int debitQuantity = product.Quantity + 1;

        DomainException domainException = Assert.Throws<DomainException>(() => product.DebitQuantity(debitQuantity));

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageQuantityDebitedLargerThanAvailable, domainException.Message);
    }

    [Fact]
    public void AddQuantity_ValidQuantity_MustAddProductQuantity()
    {
        Product product = _productFixture.GenerateProduct();
        int finalQuantity = product.Quantity + 1;

        product.AddQuantity(1);

        Assert.Equal(finalQuantity, product.Quantity);
    }

    [Fact]
    public void AddQuantity_QuantityAddZero_MustReturnDomainException()
    {
        Product product = _productFixture.GenerateProduct();

        DomainException domainException = Assert.Throws<DomainException>(() => product.AddQuantity(0));

        Assert.NotNull(domainException);
        Assert.Equal(Product.ErrorMessageQuantityAddedLessThanOrEqualToZero, domainException.Message);
    }
}
