namespace EM.Catalog.Domain;

public sealed record ErrorMessage
{
    #region Product
    public const string ProductNameNullOrEmpty = "The product name cannot be null or empty.";
    public const string ProductDescriptionNullOrEmpty = "The product description cannot be null or empty.";
    public const string ProductValueLessThanEqualToZero = "The product value cannot be less than or equal to zero.";
    public const string ProductQuantityLessThanEqualToZero = "The product quantity cannot be less than or equal to zero.";
    public const string ProductImageNullOrEmpty = "The product image cannot be null or empty.";
    public const string ProductQuantityDebitedLessThanOrEqualToZero = "The product quantity debited cannot be less than or equal to zero.";
    public const string ProductQuantityDebitedLargerThanAvailable = "The product quantity debited cannot be larger than available.";
    public const string ProductQuantityAddedLessThanOrEqualToZero = "The product quantity added cannot be less than or equal to zero.";
    public const string ProductCategoryNull = "The product category cannot be null.";
    public const string ProductInvalidId = "The product id cannot be invalid.";
    public const string ProductInvalidCategoryId = "The product category id cannot be invalid.";
    public const string ProductCategoryNotFound = "The product category not found.";
    #endregion

    #region Category
    public const string CategoryCodeLessThanEqualToZero = "The category code cannot be less than or equal to zero.";
    public const string CategoryNameNullOrEmpty = "The category name cannot be null or empty.";
    public const string CategoryDescriptionNullOrEmpty = "The category description cannot be null or empty.";
    public const string CategoryInvalidId = "The category id cannot be invalid.";
    #endregion
}