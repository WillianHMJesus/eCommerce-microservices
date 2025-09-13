using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.Fixtures;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

public class StringSpecimenBuilder : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    private readonly string _password = PasswordFixture.GeneratePassword(12);

    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type != typeof(string))
        {
            return new NoSpecimen();
        }

        string propertyName = request?.ToString()?.Replace("System.String ", "") ?? "";

        return propertyName.ToLower() switch
        {
            "emailaddress" => _faker.Internet.Email(),
            "usernamegreaterthanmaxlenght" => _faker.Random.String2(User.UserNameMaxLenght + 1),
            "emailaddressgreaterthanmaxlenght" => _faker.Random.String2(Email.EmailAddressMaxLenght + 1),
            "profilenamegreaterthanmaxlenght" => _faker.Random.String2(Profile.NameMaxLenght + 1),
            "rolenamegreaterthanmaxlenght" => _faker.Random.String2(Role.NameMaxLenght + 1),
            "password" => _password,
            "confirmpassword" => _password,
            "invalidpassword" => _faker.Lorem.Word(),
           _ => new NoSpecimen()
        };
    }
}
