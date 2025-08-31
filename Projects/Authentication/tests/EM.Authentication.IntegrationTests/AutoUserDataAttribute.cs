using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace EM.Authentication.IntegrationTests;

public class AutoUserDataAttribute : AutoDataAttribute
{
    public AutoUserDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        fixture.Customizations.Add(new CustomSpecimenBuilder());

        return fixture;
    }
}
