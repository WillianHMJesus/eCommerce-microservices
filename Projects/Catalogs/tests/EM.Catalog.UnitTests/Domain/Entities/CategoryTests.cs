using AutoFixture;
using AutoFixture.Xunit2;
using Bogus;
using Bogus.DataSets;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Xml.Linq;
using WH.SharedKernel;
using Xunit;

namespace EM.Catalog.UnitTests.Domain.Entities;

#pragma warning disable CS8625
public sealed class CategoryTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:ValidProduct")]
    public void Validate_ValidCategory_ShouldNotReturnDomainException(Category category)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(category.Validate);

        //Assert
        domainException.Should().BeNull();
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:InvalidCategoryId")]
    public void Validate_InvalidCategoryId_ShouldReturnDomainException(short code, string name, string description)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Category.Load(default, code, name, description));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be("The id is invalid");
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:ZeroCategoryCode")]
    public void Validate_ZeroCategoryCode_ShouldReturnDomainException(string name, string description)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Category.Create(default, name, description));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.CodeLessThanEqualToZero);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:EmptyCategoryName")]
    public void Validate_EmptyCategoryName_ShouldReturnDomainException(short code, string description)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Category.Create(code, default, description));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.NameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:NullCategoryName")]
    public void Validate_NullCategoryName_ShouldReturnDomainException(short code, string description)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Category.Create(code, null, description));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.NameNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:NameGreaterThanMaxLenght")]
    public void Validate_NameGreaterThanMaxLenght_ShouldReturnDomainException(Faker faker, short code, string description)
    {
        //Arrange
        string name = faker.Random.String2(Category.NameMaxLenght + 1);

        //Act
        Exception domainException = Record.Exception(() => Category.Create(code, name, description));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.NameMaxLenghtError);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:EmptyCategoryDescription")]
    public void Validate_EmptyCategoryDescription_ShouldReturnDomainException(short code, string name)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Category.Create(code, name, default));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.DescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:NullCategoryDescription")]
    public void Validate_NullCategoryDescription_ShouldReturnDomainException(short code, string name)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => Category.Create(code, name, null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.DescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Validate:DescriptionGreaterThanMaxLenght")]
    public void Validate_DescriptionGreaterThanMaxLenght_ShouldReturnDomainException(Faker faker, short code, string name)
    {
        //Arrange
        string description = faker.Random.String2(Category.DescriptionMaxLenght + 1);

        //Act
        Exception domainException = Record.Exception(() => Category.Create(code, name, description));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Category.DescriptionMaxLenghtError);
    }
}
#pragma warning restore CS8625