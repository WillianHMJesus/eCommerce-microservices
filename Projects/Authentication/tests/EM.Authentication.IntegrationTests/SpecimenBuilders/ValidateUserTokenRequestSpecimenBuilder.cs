using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class ValidateUserTokenRequestSpecimenBuilder : ISpecimenBuilder
{
    private static readonly Faker _faker = new();
    private static readonly Guid UserTokenId = Guid.Parse("c0b4e59f-bbce-4f63-a6ba-9dffd0fdc171");
    private static readonly Guid UserTokenIdExpired = Guid.Parse("6bd97845-3e70-469c-8cb9-5192b027a2b4");
    private const string Token = "123Abc";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(ValidateUserTokenRequest)))
        {
            return new NoSpecimen();
        }

        string ValidateUserTokenRequestStringType = "EM.Authentication.API.Users.RequestModels.ValidateUserTokenRequest ";
        string parameterName = request?.ToString()?.Replace(ValidateUserTokenRequestStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "request" => new ValidateUserTokenRequest { UserTokenId = UserTokenId, Token = Token },
            "requestusertokennotfound" => new ValidateUserTokenRequest { UserTokenId = Guid.NewGuid(), Token = Token },
            "requestusertokenexpired" => new ValidateUserTokenRequest { UserTokenId = UserTokenIdExpired, Token = _faker.System.ApplePushToken() },
            _ => new NoSpecimen()
        };
    }
}
