using AutoMapper;
using EM.Carts.Application.Mappings;
using Xunit;

namespace EM.Carts.UnitTests.Application.Mappings;

public sealed class CartMappingTest
{
    [Fact]
    public void Constructor_ConfigurationIsValid_ShouldNotThrowException()
    {
        var configuration = new MapperConfiguration(x => x.AddProfile<CartMapping>());
        configuration.AssertConfigurationIsValid();
    }
}
