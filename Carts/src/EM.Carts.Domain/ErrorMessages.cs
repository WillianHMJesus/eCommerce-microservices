namespace EM.Carts.Domain;

public sealed record ErrorMessage
{
    public const string UserIdInvalid = "The user id cannot be invalid";
    public const string CartItemNull = "The cart item cannot be null";
    public const string CartNotFound = "The cart not found";

    public const string ProductIdInvalid = "The product id cannot be invalid";
    public const string ProductNameNullOrEmpty = "The product name cannot be null or empty.";
    public const string ProductImageNullOrEmpty = "The product image cannot be null or empty.";
    public const string ValueLessThanEqualToZero = "The value cannot be less than or equal to zero.";
    public const string QuantityLessThanEqualToZero = "The quantity cannot be less than or equal to zero.";
    public const string QuantityGreaterThanAvailable = "The quantity cannot be greater than the available quantity.";
    public const string ItemNotFound = "The item not found";
}
