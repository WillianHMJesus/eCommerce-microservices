using Bogus;
using EM.Catalog.Domain;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using WH.SharedKernel;
using Xunit;

namespace EM.Catalog.UnitTests.Domain;

#pragma warning disable CS8625
public sealed class ProductTests
{
    [Theory, AutoProductData]
    [Trait("Test", "Validate:ValidProduct")]
    public void Validate_ValidProduct_ShouldNotReturnDomainException(Product product)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(product.Validate);

        //Assert
        domainException.Should().BeNull();
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:InvalidProductId")]
    public void Validate_InvalidProductId_ShouldReturnDomainException(string name, string description, decimal value, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Load(default, name, description, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be("The id is invalid");
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:EmptyProductName")]
    public void Validate_EmptyProductName_ShouldReturnDomainException(string description, decimal value, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create("", description, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.NameNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:NullProductName")]
    public void Validate_NullProductName_ShouldReturnDomainException(string description, decimal value, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(null, description, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.NameNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:NameGreaterThanMaxLenght")]
    public void Validate_NameGreaterThanMaxLenght_ShouldReturnDomainException(Faker faker, string description, decimal value, string image)
    {
        //Arrange
        string name = faker.Random.String2(Product.NameMaxLenght + 1);

        //Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.NameMaxLenghtError);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:EmptyProductDescription")]
    public void Validate_EmptyProductDescription_ShouldReturnDomainException(string name, decimal value, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(name, "", value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.DescriptionNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:NullProductDescription")]
    public void Validate_NullProductDescription_ShouldReturnDomainException(string name, decimal value, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(name, null, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.DescriptionNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:DescriptionGreaterThanMaxLenght")]
    public void Validate_DescriptionGreaterThanMaxLenght_ShouldReturnDomainException(Faker faker, string name, decimal value, string image)
    {
        //Arrange
        string description = faker.Random.String2(Product.DescriptionMaxLenght + 1);

        //Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.DescriptionMaxLenghtError);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:ZeroProductValue")]
    public void Validate_ZeroProductValue_ShouldReturnDomainException(string name, string description, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, 0, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ValueLessThanEqualToZero);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:EmptyProductImage")]
    public void Validate_EmptyProductImage_ShouldReturnDomainException(string name, string description, decimal value)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, value, "", Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:NullProductImage")]
    public void Validate_NullProductImage_ShouldReturnDomainException(string name, string description, decimal value)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, value, null, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:ImageGreaterThanMaxLenght")]
    public void Validate_ImageGreaterThanMaxLenght_ShouldReturnDomainException(Faker faker, string name, string description, decimal value)
    {
        //Arrange
        string image = faker.Random.String2(Product.ImageMaxLenght + 1);

        //Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, value, image, Guid.NewGuid()));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.ImageMaxLenghtError);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Validate:DefaultProductCategoryId")]
    public void Validate_DefaultProductCategoryId_ShouldReturnDomainException(string name, string description, decimal value, string image)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Product.Create(name, description, value, image, default));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.InvalidCategoryId);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Reactivate:InvalidProduct")]
    public void Reactivate_InvalidProduct_ShouldReactivateProduct(Product product)
    {
        //Arrange & Act
        product.Reactivate();

        //Assert
        product.Available.Should().BeTrue();
    }

    [Theory, AutoProductData]
    [Trait("Test", "Inactivate:ValidProduct")]
    public void Inactivate_ValidProduct_ShouldInactivateProduct(Product product)
    {
        //Arrange & Act
        product.Inactivate();

        //Assert
        product.Available.Should().BeFalse();
    }

    [Theory, AutoProductData]
    [Trait("Test", "AddQuantity:ValidQuantity")]
    public void AddQuantity_ValidQuantity_ShouldAddProductQuantity(Product product, short quantityAdded)
    {
        //Arrange
        short currentQuantity = product.Quantity;

        //Act
        product.AddQuantity(quantityAdded);

        //Assert
        product.Quantity.Should().Be((short)(currentQuantity + quantityAdded));
    }

    [Theory, AutoProductData]
    [Trait("Test", "AddQuantity:QuantityAddZero")]
    public void AddQuantity_QuantityAddZero_ShouldReturnDomainException(Product product)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => product.AddQuantity(0));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.QuantityAddedLessThanOrEqualToZero);
    }

    [Theory, AutoProductData]
    [Trait("Test", "RemoveQuantity:ValidQuantity")]
    public void RemoveQuantity_ValidQuantity_ShouldRemoveProductQuantity(Product product, short quantityAdded)
    {
        //Arrange
        product.AddQuantity(quantityAdded);

        //Act
        product.RemoveQuantity(quantityAdded);

        //Assert
        product.Quantity.Should().Be(0);
    }

    [Theory, AutoProductData]
    [Trait("Test", "RemoveQuantity:QuantityDebitedZero")]
    public void RemoveQuantity_QuantityDebitedZero_ShouldReturnDomainException(Product product)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => product.RemoveQuantity(0));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.QuantityDebitedLessThanOrEqualToZero);
    }

    [Theory, AutoProductData]
    [Trait("Test", "RemoveQuantity:ProductQuantityZero")]
    public void RemoveQuantity_ProductQuantityZero_ShouldReturnDomainException(Product product)
    {
        //Arrange
        short debitQuantity = (short)(product.Quantity + 1);

        //Act
        Exception domainException = Record.Exception(() => product.RemoveQuantity(debitQuantity));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Product.QuantityDebitedGreaterThanAvailable);
    }
}
#pragma warning restore CS8625