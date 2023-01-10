using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using Xunit;

namespace EM.Catalog.UnitTests.Domain;

public class CategoryTest
{
    [Fact]
    public void Validate_ValidCategory_MustNotReturnDomainException()
    {
        Exception domainException = Record.Exception(() 
            => new Category(10, "Voucher Virtual", "Voucher virtual são cartões presente para consumir em loja"));

        Assert.Null(domainException);
    }

    [Fact]
    public void Validate_InvalidCategoryCode_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(() 
            => new Category(0, "Voucher Virtual", "Voucher virtual são cartões presente para consumir em loja"));

        Assert.NotNull(domainException);
        Assert.Equal(Category.ErrorMessageCodeLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidCategoryName_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => new Category(10, "", "Voucher virtual são cartões presente para consumir em loja"));

        Assert.NotNull(domainException);
        Assert.Equal(Category.ErrorMessageNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidCategoryDescription_MustReturnDomainException()
    {
        DomainException domainException = Assert.Throws<DomainException>(()
            => new Category(10, "Voucher Virtual", ""));

        Assert.NotNull(domainException);
        Assert.Equal(Category.ErrorMessageDescriptionNullOrEmpty, domainException.Message);
    }
}
