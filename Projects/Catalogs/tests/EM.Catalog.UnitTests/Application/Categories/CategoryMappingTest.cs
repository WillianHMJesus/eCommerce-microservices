using AutoMapper;
using EM.Catalog.Application.Categories;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories;

public sealed class CategoryMappingTest
{
    [Fact]
    public void Constructor_ConfigurationIsValid_ShouldNotThrowException()
    {
        var configuration = new MapperConfiguration(x => x.AddProfile<CategoryMapping>());
        configuration.AssertConfigurationIsValid();
    }
}
