using EM.Catalog.Domain.Entities;
using WH.SharedKernel;

namespace EM.Catalog.Domain;

public class Product : BaseEntity, IAggregateRoot
{
    public const int NameMaxLenght = 50;
    public const int ImageMaxLenght = 50;
    public const int DescriptionMaxLenght = 100;
    public const string InvalidProductId = "The productId is invalid";
    public const string ProductNotFound = "the product was not found";
    public const string NameNullOrEmpty = "The name cannot be null or empty";
    public const string ErrorSavingProduct = "An error occurred while saving the product";
    public const string ImageNullOrEmpty = "The image cannot be null or empty";
    public const string InvalidCategoryId = "Tha categoryId is invalid";
    public const string DescriptionNullOrEmpty = "The description cannot be null or empty";
    public const string ValueLessThanEqualToZero = "The value cannot be less than or equal to zero";
    public const string QuantityDebitedLessThanOrEqualToZero = "The quantity debited cannot be less than or equal to zero";
    public const string QuantityDebitedGreaterThanAvailable = "The quantity debited cannot be greater than available";
    public const string QuantityAddedLessThanOrEqualToZero = "The quantity added cannot be less than or equal to zero";
    public const string ProductHasAlreadyBeenRegistered = "The product has already been registered";
    public static readonly string NameMaxLenghtError = $"The name cannot be greater than {NameMaxLenght}";
    public static readonly string DescriptionMaxLenghtError = $"The description cannot be greater than {DescriptionMaxLenght}";
    public static readonly string ImageMaxLenghtError = $"The image cannot be greater than {ImageMaxLenght}";

    private Product(Guid id, string name, string description, decimal value, string image, Guid categoryId)
        : base(id)
    {
        Name = name;
        Description = description;
        Value = value;
        Image = image;
        CategoryId = categoryId;
        Quantity = 0;
        Available = true;

        Validate();
    }

    public string Name { get; init; } = ""!;
    public string Description { get; init; } = ""!;
    public decimal Value { get; init; }
    public short Quantity { get; private set; }
    public string Image { get; init; } = ""!;
    public bool Available { get; private set; }
    public Guid CategoryId { get; init; }
    public Category? Category { get; private set; }

    public static Product Create(string name, string description, decimal value, string image, Guid categoryId)
    {
        return new Product(Guid.NewGuid(), name, description, value, image, categoryId);
    }

    public static Product Load(Guid id, string name, string description, decimal value, string image, Guid categoryId)
    {
        return new Product(id, name, description, value, image, categoryId);
    }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrEmpty(Name, NameNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Name, NameMaxLenght, NameMaxLenghtError);
        AssertionConcern.ValidateNullOrEmpty(Description, DescriptionNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Description, DescriptionMaxLenght, DescriptionMaxLenghtError);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ValueLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Image, ImageNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Image, ImageMaxLenght, ImageMaxLenghtError);
        AssertionConcern.ValidateNullOrDefault(CategoryId, InvalidCategoryId);
    }

    public void Reactivate() => Available = true;

    public void Inactivate() => Available = false;

    public void RemoveQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, QuantityDebitedLessThanOrEqualToZero);
        AssertionConcern.ValidateLessThanMinimum(Quantity, quantity, QuantityDebitedGreaterThanAvailable);
        Quantity -= quantity;
    }

    public void AddQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, QuantityAddedLessThanOrEqualToZero);
        Quantity += quantity;
    }
}
