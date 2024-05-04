namespace EM.Checkout.Domain;

public class ErrorMessage
{
    public const string UserIdInvalid = "The user id cannot be invalid";
    public const string OrderItemNull = "The order item cannot be null";

    public const string ProductIdInvalid = "The product id cannot be invalid";
    public const string ProductNameNullOrEmpty = "The product name cannot be null or empty.";
    public const string ProductImageNullOrEmpty = "The product image cannot be null or empty.";
    public const string ValueLessThanEqualToZero = "The value cannot be less than or equal to zero.";
    public const string QuantityLessThanEqualToZero = "The quantity cannot be less than or equal to zero.";

    public const string CartNotFound = "The cart not found";

    public const string CardHolderCpfInvalid = "The card holder CPF cannot be invalid";
    public const string CardHolderNameInvalid = "The card holder name cannot be invalid";
    public const string CardNumberInvalid = "The card number cannot be invalid";
    public const string CardExpirationDateInvalid = "The card expiration date cannot be invalid";
    public const string CardSecurityCodeInvalid = "The card security code cannot be invalid";
}
