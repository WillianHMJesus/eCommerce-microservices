using AutoFixture.Kernel;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests;

public class CustomSpecimenBuilder : ISpecimenBuilder
{
    private readonly UserFixture _fixture = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request.ToString()!.Contains(nameof(AddCustomerRequest)))
        {
            string AddUserRequestStringType = "EM.Authentication.API.Users.RequestModels.AddCustomerRequest ";
            string parameterName = request?.ToString()?.Replace(AddUserRequestStringType, "").Trim() ?? "";

            return parameterName.ToLower() switch
            {
                "request" => _fixture.GetValidAddCustomerRequest(),
                _ => new NoSpecimen()
            };
        }
        else if (request.ToString()!.Contains(nameof(AddUserRequest)))
        {
            string AddUserRequestStringType = "EM.Authentication.API.Users.RequestModels.AddUserRequest ";
            string parameterName = request?.ToString()?.Replace(AddUserRequestStringType, "").Trim() ?? "";

            return parameterName.ToLower() switch
            {
                "request" => _fixture.GetValidAddUserRequest(),
                "nullvalues" => _fixture.GetAddUserRequestNullValues(),
                "largerthanmaxlenght" => _fixture.GetAddUserRequestLargerThanMaxLenght(),
                "invalidvalues" => _fixture.GetAddUserRequestInvalidValues(),
                "differentpasswords" => _fixture.GetAddUserRequestDifferentPasswords(),
                "profilenamenotfound" => _fixture.GetAddUserRequestProfileNameNotFound(),
                _ => new NoSpecimen()
            };
        }
        else if (request.ToString()!.Contains(nameof(AuthenticateUserRequest)))
        {
            string AddUserRequestStringType = "EM.Authentication.API.Users.RequestModels.AuthenticateUserRequest ";
            string parameterName = request?.ToString()?.Replace(AddUserRequestStringType, "").Trim() ?? "";

            return parameterName.ToLower() switch
            {
                "request" => _fixture.GetValidAuthenticateUserRequest(),
                "nullvalues" => _fixture.GetAuthenticateUserRequestNullValues(),
                "requestwithemailaddressnotfound" => _fixture.GetAuthenticateUserRequestEmailAddressNotFound(),
                "requestincorrectpassword" => _fixture.GetAuthenticateUserRequestIncorrectPassword(),
                _ => new NoSpecimen()
            };
        }

        return new NoSpecimen();
    }
}