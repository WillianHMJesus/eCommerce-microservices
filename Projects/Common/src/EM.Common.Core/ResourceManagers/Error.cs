namespace EM.Common.Core.ResourceManagers;

public sealed record Error(string Key, string Message);

public sealed record Key
{
    #region Product
    public const string ProductNameNullOrEmpty = nameof(ProductNameNullOrEmpty);
    public const string ProductDescriptionNullOrEmpty = nameof(ProductDescriptionNullOrEmpty);
    public const string ProductValueLessThanEqualToZero = nameof(ProductValueLessThanEqualToZero);
    public const string ProductQuantityLessThanEqualToZero = nameof(ProductQuantityLessThanEqualToZero);
    public const string ProductImageNullOrEmpty = nameof(ProductImageNullOrEmpty);
    public const string ProductQuantityDebitedLessThanOrEqualToZero = nameof(ProductQuantityDebitedLessThanOrEqualToZero);
    public const string ProductQuantityDebitedLargerThanAvailable = nameof(ProductQuantityDebitedLargerThanAvailable);
    public const string ProductQuantityAddedLessThanOrEqualToZero = nameof(ProductQuantityAddedLessThanOrEqualToZero);
    public const string ProductInvalidId = nameof(ProductInvalidId);
    public const string ProductInvalidCategoryId = nameof(ProductInvalidCategoryId);
    public const string ProductCategoryNotFound = nameof(ProductCategoryNotFound);
    public const string ProductAnErrorOccorred = nameof(ProductAnErrorOccorred);
    public const string ProductRegisterDuplicity = nameof(ProductRegisterDuplicity);
    public const string ProductNotFound = nameof(ProductNotFound);
    #endregion

    #region Category
    public const string CategoryCodeLessThanEqualToZero = nameof(CategoryCodeLessThanEqualToZero);
    public const string CategoryNameNullOrEmpty = nameof(CategoryNameNullOrEmpty);
    public const string CategoryDescriptionNullOrEmpty = nameof(CategoryDescriptionNullOrEmpty);
    public const string CategoryInvalidId = nameof(CategoryInvalidId);
    public const string CategoryAnErrorOccorred = nameof(CategoryAnErrorOccorred);
    public const string CategoryRegisterDuplicity = nameof(CategoryRegisterDuplicity);
    public const string CategoryNotFound = nameof(CategoryNotFound);
    #endregion
}
