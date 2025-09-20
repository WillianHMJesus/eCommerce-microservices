using WH.SharedKernel;

namespace EM.Authentication.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public const int EmailAddressMaxLenght = 100;
    public const string EmailAddressNullOrEmpty = "The email address cannot be null or empty";
    public const string InvalidEmailAddress = "The email address is invalid";
    public const string EmailAddressHasAlreadyBeenRegistered = "The email address has already been registered";
    public static readonly string EmailAddressMaxLenghtError = $"The email address cannot be greater than {EmailAddressMaxLenght}";

    public Email(string emailAddress)
    {
        EmailAddress = string.IsNullOrEmpty(emailAddress) ? "" : emailAddress.Trim();
        Validate();
    }

    public string EmailAddress { get; init; }

    public void Validate()
    {
        AssertionConcern.ValidateNullOrWhiteSpace(EmailAddress, EmailAddressNullOrEmpty);
        AssertionConcern.ValidateMaxLength(EmailAddress, EmailAddressMaxLenght, EmailAddressMaxLenghtError);
        AssertionConcern.ValidateEmailAddress(EmailAddress, InvalidEmailAddress);
    }

    public override string ToString() => EmailAddress;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EmailAddress;
    }

    public static explicit operator Email(string emailAddress) => new Email(emailAddress);
}
