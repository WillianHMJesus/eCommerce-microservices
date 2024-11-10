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
    public const string ProductUnavailable = nameof(ProductUnavailable);
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

    #region Cart
    public const string CartItemNull = nameof(CartItemNull);
    public const string CartNotFound = nameof(CartNotFound);
    #endregion

    #region Item
    public const string QuantityGreaterThanAvailable = nameof(QuantityGreaterThanAvailable);
    public const string ItemNotFound = nameof(ItemNotFound);
    public const string ValueInvalid = nameof(ValueInvalid);
    public const string StatusInvalid = nameof(StatusInvalid);
    #endregion

    #region Card
    public const string CardHolderCpfInvalid = nameof(CardHolderCpfInvalid);
    public const string CardHolderNameInvalid = nameof(CardHolderNameInvalid);
    public const string CardNumberInvalid = nameof(CardNumberInvalid);
    public const string CardExpirationDateInvalid = nameof(CardExpirationDateInvalid);
    public const string CardSecurityCodeInvalid = nameof(CardSecurityCodeInvalid);
    #endregion

    #region Order
    public const string OrderNumberNull = nameof(OrderNumberNull);
    public const string OrderItemNull = nameof(OrderItemNull);
    public const string OrderIdInvalid = nameof(OrderIdInvalid);
    #endregion

    #region User
    public const string UserIdInvalid = nameof(UserIdInvalid);
    #endregion
}
