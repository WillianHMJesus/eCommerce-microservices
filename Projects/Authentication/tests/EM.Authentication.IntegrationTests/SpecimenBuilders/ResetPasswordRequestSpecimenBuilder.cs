using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

#pragma warning disable CS8625

public sealed class ResetPasswordRequestSpecimenBuilder : ISpecimenBuilder
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
            "request" => new ResetPasswordRequest { UserTokenId = UserTokenId, NewPassword = NewPassword, ConfirmPassword = NewPassword },
            "requestdefaultnewpassword" => new ResetPasswordRequest { UserTokenId = UserTokenId, NewPassword = default, ConfirmPassword = default },
            "requestnullnewpassword" => new ResetPasswordRequest { UserTokenId = UserTokenId, NewPassword = null, ConfirmPassword = null },
            "requestinvalidnewpassword" => new ResetPasswordRequest { UserTokenId = UserTokenId, NewPassword = _faker.Lorem.Word(), ConfirmPassword = _faker.Lorem.Word() },
            "requestdifferentpasswords" => new ResetPasswordRequest { UserTokenId = UserTokenId, NewPassword = NewPassword, ConfirmPassword = _faker.Lorem.Word() },
            "requestusertokennotfound" => new ResetPasswordRequest { UserTokenId = Guid.NewGuid(), NewPassword = NewPassword, ConfirmPassword = NewPassword },
            "requestusertokennotvalidated" => new ResetPasswordRequest { UserTokenId = UserTokenIdNotValidated, NewPassword = NewPassword, ConfirmPassword = NewPassword },
            _ => new NoSpecimen()
        };
    }
}
#pragma warning restore CS8625