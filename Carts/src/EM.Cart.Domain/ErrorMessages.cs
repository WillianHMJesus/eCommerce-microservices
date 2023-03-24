namespace EM.Cart.Domain;

public class ErrorMessage
{
    public const string UserIdInvalid = "The user id cannot be invalid";
    public const string CartItemNull = "The cart item cannot be null";

    public const string ProductIdInvalid = "The product id cannot be invalid";
    public const string ProductNameNullOrEmpty = "The product name cannot be null or empty.";
    public const string ProductImageNullOrEmpty = "The product image cannot be null or empty.";
    public const string ValueLessThanEqualToZero = "The value cannot be less than or equal to zero.";
    public const string QuantityLessThanEqualToZero = "The quantity cannot be less than or equal to zero.";
}
