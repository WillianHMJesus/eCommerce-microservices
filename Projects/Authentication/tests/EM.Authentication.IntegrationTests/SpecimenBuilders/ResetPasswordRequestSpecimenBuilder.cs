using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class ResetPasswordRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private static readonly Faker _faker = new();
    private static readonly Guid UserTokenId = Guid.Parse("2a20862f-1dce-451e-9e81-bd755bab25fe");
    private static readonly Guid UserTokenIdNotValidated = Guid.Parse("f6cdc732-908a-4e0d-9d7c-1e04d1512bf9");
    private const string NewPassword = "Abc987654@";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(ResetPasswordRequest)))
        {
            return new NoSpecimen();
        }

        string ResetPasswordRequestStringType = "EM.Authentication.API.Users.RequestModels.ResetPasswordRequest ";
        string parameterName = request?.ToString()?.Replace(ResetPasswordRequestStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "request" => GetRequest(),
            "requestdefaultnewpassword" => GetRequestDefaultNewPassword(),
            "requestnullnewpassword" => GetRequestNullNewPassword(),
            "requestinvalidnewpassword" => GetInvalidNewPassword(),
            "requestdifferentpasswords" => GetDifferentPasswords(),
            "requestusertokennotfound" => GetUserTokenNotFound(),
            "requestusertokennotvalidated" => GetUserTokenNotValidated(),
            _ => new NoSpecimen()
        };
    }

    private ResetPasswordRequest GetRequest() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ResetPasswordRequest GetRequestDefaultNewPassword() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .With(x => x.NewPassword, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .Create();

    private ResetPasswordRequest GetRequestNullNewPassword() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .With(x => x.NewPassword, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .Create();

    private ResetPasswordRequest GetInvalidNewPassword() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .With(x => x.NewPassword, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ResetPasswordRequest GetDifferentPasswords() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, UserTokenId)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, _faker.Lorem.Word())
            .Create();

    private ResetPasswordRequest GetUserTokenNotFound() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, Guid.NewGuid())
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ResetPasswordRequest GetUserTokenNotValidated() =>
        fixture.Build<ResetPasswordRequest>()
            .With(x => x.UserTokenId, UserTokenIdNotValidated)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();
}