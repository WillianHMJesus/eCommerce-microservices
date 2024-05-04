using AutoFixture;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Shared.Core;
using FluentAssertions;
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
        domainException.Message.Should().Be(ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductName_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Name, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_EmptyProductDescription_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Description, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductDescription_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Description, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductDescriptionNullOrEmpty);
    }

    [Fact]
    public void Validate_ZeroProductValue_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Value, 0)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public void Validate_EmptyProductImage_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Image, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductImage_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.Image, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Validate_DefaultProductCategoryId_ShouldReturnDomainException()
    {
        Product product = _fixture.Build<Product>()
            .With(x => x.CategoryId, Guid.Empty)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => product.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductInvalidCategoryId);
    }

    [Fact]
    public void MakeAvailable_InvalidProduct_ShouldMakeAvailableProduct()
    {
        Product product = _fixture.Create<Product>();

        product.MakeAvailable();

        product.Available.Should().BeTrue();
    }

    [Fact]
    public void MakeUnavailable_ValidProduct_ShouldMakeUnavailableProduct()
    {
        Product product = _fixture.Create<Product>();

        product.MakeUnavailable();

        product.Available.Should().BeFalse();
    }

    [Fact]
    public void AddQuantity_ValidQuantity_ShouldAddProductQuantity()
    {
        Product product = _fixture.Create<Product>();
        short quantityAdded = _fixture.Create<short>();

        product.AddQuantity(quantityAdded);

        product.Quantity.Should().Be(quantityAdded);
    }

    [Fact]
    public void AddQuantity_QuantityAddZero_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();

        DomainException domainException = Assert.Throws<DomainException>(() => product.AddQuantity(0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductQuantityAddedLessThanOrEqualToZero);
    }

    [Fact]
    public void RemoveQuantity_ValidQuantity_ShouldRemoveProductQuantity()
    {
        Product product = _fixture.Create<Product>();
        product.AddQuantity(_fixture.Create<short>());

        product.RemoveQuantity(product.Quantity);

        product.Quantity.Should().Be(0);
    }

    [Fact]
    public void RemoveQuantity_QuantityDebitedZero_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();

        DomainException domainException = Assert.Throws<DomainException>(() => product.RemoveQuantity(0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductQuantityDebitedLessThanOrEqualToZero);
    }

    [Fact]
    public void RemoveQuantity_ProductQuantityZero_ShouldReturnDomainException()
    {
        Product product = _fixture.Create<Product>();
        short debitQuantity = (short)(product.Quantity + 1);

        DomainException domainException = Assert.Throws<DomainException>(() => product.RemoveQuantity(debitQuantity));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.ProductQuantityDebitedLargerThanAvailable);
    }
}
