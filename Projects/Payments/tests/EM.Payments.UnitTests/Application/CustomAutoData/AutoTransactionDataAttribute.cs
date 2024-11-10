using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace EM.Payments.UnitTests.Application.CustomAutoData;

public class AutoTransactionDataAttribute : AutoDataAttribute
{
    public AutoTransactionDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        return fixture;
    }
}
