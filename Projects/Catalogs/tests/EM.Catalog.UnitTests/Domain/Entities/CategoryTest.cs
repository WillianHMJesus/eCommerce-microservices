using AutoFixture;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Common.Core.Domain;
using FluentAssertions;
using Xunit;

namespace EM.Catalog.UnitTests.Domain.Entities;

public sealed class CategoryTest
{
    private readonly Fixture _fixture;

    public CategoryTest() => _fixture = new();

    [Fact]
    public void Validate_ValidCategory_ShouldNotReturnDomainException()
    {
        Category category = _fixture.Create<Category>();

        Exception domainException = Record.Exception(() => category.Validate());

        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_ZeroCategoryCode_ShouldReturnDomainException()
    {
        Category category = _fixture.Build<Category>()
            .With(x => x.Code, 0)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => category.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryCodeLessThanEqualToZero);
    }

    [Fact]
    public void Validate_EmptyCategoryName_ShouldReturnDomainException()
    {
        Category category = _fixture.Build<Category>()
            .With(x => x.Name, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => category.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullCategoryName_ShouldReturnDomainException()
    {
        Category category = _fixture.Build<Category>()
            .With(x => x.Name, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => category.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public void Validate_EmptyCategoryDescription_ShouldReturnDomainException()
    {
        Category category = _fixture.Build<Category>()
            .With(x => x.Description, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => category.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public void Validate_NullCategoryDescription_ShouldReturnDomainException()
    {
        Category category = _fixture.Build<Category>()
            .With(x => x.Description, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => category.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(ErrorMessage.CategoryDescriptionNullOrEmpty);
    }
}
