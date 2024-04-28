using AutoMapper;
using EM.Catalog.Application.Products;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products;

public sealed class ProductMappingTest
{
    [Fact]
    public void Constructor_ConfigurationIsValid_ShouldNotThrowException()
    {
        var configuration = new MapperConfiguration(x => x.AddProfile<ProductMapping>());
        configuration.AssertConfigurationIsValid();
    }
}
