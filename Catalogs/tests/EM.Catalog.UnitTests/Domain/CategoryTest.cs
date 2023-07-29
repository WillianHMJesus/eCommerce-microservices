using EM.Catalog.Domain;
using EM.Catalog.UnitTests.Fixtures;
using Xunit;

namespace EM.Catalog.UnitTests.Domain;

public sealed class CategoryTest
{
    private readonly CategoryFixture _categoryFixture;

    public CategoryTest()
        => _categoryFixture = new CategoryFixture();

    [Fact]
    public void Validate_ValidCategory_MustNotReturnDomainException()
    {
        Exception domainException = Record.Exception(() 
            => _categoryFixture.GenerateCategory());

        Assert.Null(domainException);
    }

    [Fact]
    public void Validate_InvalidCategoryCode_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(() 
            => _categoryFixture.GenerateCategoryWithInvalidCode());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.CategoryCodeLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidCategoryName_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _categoryFixture.GenerateCategoryWithInvalidName());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.CategoryNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidCategoryDescription_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => _categoryFixture.GenerateCategoryWithInvalidDescription());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.CategoryDescriptionNullOrEmpty, domainException.Message);
    }
}
