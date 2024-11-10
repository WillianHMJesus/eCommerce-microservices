using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using Xunit;

namespace EM.Catalog.UnitTests.Domain.Entities;

public sealed class ProductTest
{
    private readonly Fixture _fixture;

    public ProductTest() => _fixture = new();

    [Theory, AutoData]
    public void Validate_ValidProduct_ShouldNotReturnDomainException(Product product)
    {
        Exception domainException = Record.Exception(() => product.Validate());

        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_EmptyProductName_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Name, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductName_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Name, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_EmptyProductDescription_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Description, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductDescription_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Description, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Validate_ZeroProductValue_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Value, 0)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public void Validate_EmptyProductImage_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Image, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductImage_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Image, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Validate_DefaultProductCategoryId_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductInvalidCategoryId);
    }

    [Theory, AutoData]
    public void MakeAvailable_InvalidProduct_ShouldMakeAvailableProduct(Product product)
    {
        product.MakeAvailable();

        product.Available.Should().BeTrue();
    }

    [Theory, AutoData]
    public void MakeUnavailable_ValidProduct_ShouldMakeUnavailableProduct(Product product)
    {
        product.MakeUnavailable();

        product.Available.Should().BeFalse();
    }

    [Theory, AutoData]
    public void AddQuantity_ValidQuantity_ShouldAddProductQuantity(Product product, short quantityAdded)
    {
        product.AddQuantity(quantityAdded);

        product.Quantity.Should().Be(quantityAdded);
    }

    [Theory, AutoData]
    public void AddQuantity_QuantityAddZero_ShouldReturnDomainException(Product product)
    {
        DomainException domainException = Assert.Throws<DomainException>(() => product.AddQuantity(0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityAddedLessThanOrEqualToZero);
    }

    [Theory, AutoData]
    public void RemoveQuantity_ValidQuantity_ShouldRemoveProductQuantity(Product product, short quantityAdded)
    {
        product.AddQuantity(quantityAdded);

        product.RemoveQuantity(product.Quantity);

        product.Quantity.Should().Be(0);
    }

    [Theory, AutoData]
    public void RemoveQuantity_QuantityDebitedZero_ShouldReturnDomainException(Product product)
    {
        DomainException domainException = Assert.Throws<DomainException>(() => product.RemoveQuantity(0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityDebitedLessThanOrEqualToZero);
    }

    [Theory, AutoData]
    public void RemoveQuantity_ProductQuantityZero_ShouldReturnDomainException(Product product)
    {
        short debitQuantity = (short)(product.Quantity + 1);

        DomainException domainException = Assert.Throws<DomainException>(() => product.RemoveQuantity(debitQuantity));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityDebitedLargerThanAvailable);
    }

    [Theory, AutoData]
    public void Inactivate_ValidProduct_ShouldInactivateProduct(Product product)
    {
        product.Inactivate();

        product.Active.Should().BeFalse();
    }
}
