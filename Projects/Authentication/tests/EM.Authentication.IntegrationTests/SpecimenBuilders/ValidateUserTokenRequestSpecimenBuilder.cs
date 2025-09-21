using AutoFixture;
using AutoFixture.Kernel;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class ValidateUserTokenRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
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
            "request" => GetRequest(),
            "requestusertokennotfound" => GetRequestUserTokenNotFound(),
            "requestusertokenexpired" => GetRequestUserTokenExpired(),
            "requestinvalidtoken" => GetRequestInvalidToken(),
            _ => new NoSpecimen()
        };
    }

    private ValidateUserTokenRequest GetRequest() =>
        fixture.Build<ValidateUserTokenRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .With(x => x.Token, Token)
            .Create();

    private ValidateUserTokenRequest GetRequestUserTokenNotFound() =>
        fixture.Build<ValidateUserTokenRequest>()
            .With(x => x.Token, Token)
            .Create();

    private ValidateUserTokenRequest GetRequestUserTokenExpired() =>
        fixture.Build<ValidateUserTokenRequest>()
            .With(x => x.UserTokenId, UserTokenIdExpired)
            .With(x => x.Token, Token)
            .Create();

    private ValidateUserTokenRequest GetRequestInvalidToken() =>
        fixture.Build<ValidateUserTokenRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .Create();
}
