using AutoMapper;
using EM.Payments.Application.Mappings;
using FluentAssertions;

namespace EM.Payments.UnitTests.Application.Mappings;

public sealed class TransactionMappingTest
{
    [Fact]
    public void Constructor_ConfigurationIsValid_ShouldNotThrowException()
    {
        var configuration = new MapperConfiguration(x => x.AddProfile<TransactionMapping>());
        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void MaskCardNumber_CardNumber_ShouldReturnMaskedCardNumber()
    {
        string cardNumber = "1234567812345678";
        string maskedCardNumber = "************5678";

        string result = TransactionMapping.MaskCardNumber(cardNumber);

        result.Should().Be(maskedCardNumber);
    }
}
