using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.Domain;
using AutoFixture;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class AddUserRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    public const string Password = "123456Abc*";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(AddUserRequest)))
        {
            return new NoSpecimen();
        }

        string AddUserRequestStringType = "EM.Authentication.API.Users.RequestModels.AddUserRequest ";
        string parameterName = request?.ToString()?.Replace(AddUserRequestStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "request" => GetRequest(),
            "requestdefaultvalues" => GetRequestDefaultValues(),
            "requestnullvalues" => GetRequestNullValues(),
            "requestgreaterthanmaxlenght" => GetRequestGreaterThanMaxLenght(),
            "invalidrequest" => GetInvalidRequest(),
            "requestpassworddifferent" => GetRequestPasswordDifferent(),
            "requestprofilenamenotfound" => GetRequestProfileNameNotFound(),
            _ => new NoSpecimen()
        };
    }

    private AddUserRequest GetRequest() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, "Manager")
            .Create();

    private AddUserRequest GetRequestDefaultValues() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.UserName, default(string))
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .With(x => x.ProfileName, default(string))
            .Create();

    private AddUserRequest GetRequestNullValues() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.UserName, null as string)
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .With(x => x.ProfileName, null as string)
            .Create();

    private AddUserRequest GetRequestGreaterThanMaxLenght() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.UserName, _faker.Random.String2(User.UserNameMaxLenght + 1))
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, "Manager")
            .Create();

    private AddUserRequest GetInvalidRequest() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.Password, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, "Manager")
            .Create();

    private AddUserRequest GetRequestPasswordDifferent() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, _faker.Lorem.Word())
            .With(x => x.ProfileName, "Manager")
            .Create();

    private AddUserRequest GetRequestProfileNameNotFound() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, _faker.Lorem.Word())
            .Create();
}
