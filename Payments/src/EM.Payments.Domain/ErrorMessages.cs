namespace EM.Payments.Domain;

public sealed class ErrorMessage
{
    public const string UserIdInvalid = "The user id cannot be invalid";
    public const string OrderIdInvalid = "The order id cannot be invalid";
    public const string ValueInvalid = "The value cannot be invalid";
    public const string CardNumberNullOrEmpty = "The card number cannot be null or empty";
}
