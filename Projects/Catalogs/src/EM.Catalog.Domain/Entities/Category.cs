using WH.SharedKernel;

namespace EM.Catalog.Domain.Entities;

public class Category : BaseEntity
{
    public const int NameMaxLenght = 50;
    public const int DescriptionMaxLenght = 100;
    public const string InvalidCategory = "The categoryId is invalid";
    public const string CodeLessThanEqualToZero = "The code cannot be less than zero";
    public const string NameNullOrEmpty = "The name cannot be null or empty";
    public const string DescriptionNullOrEmpty = "The description cannot be null or empty";
    public const string CategoryHasAlreadyBeenRegistered = "The category has already been registered";
    public const string ErrorSavingCategory = "An error occurred while saving the category";
    public const string CategoryInvalidId = "the category id is invalid";
    public const string CategoryNotFound = "the category was not found";
    public static readonly string NameMaxLenghtError = $"The name cannot be greater than {NameMaxLenght}";
    public static readonly string DescriptionMaxLenghtError = $"The description cannot be greater than {DescriptionMaxLenght}";

    private Category(Guid id, short code, string name, string description)
        : base(id)
    {
        Code = code;
        Name = name;
        Description = description;

        Validate();
    }

    public short Code { get; init; }
    public string Name { get; init; } = ""!;
    public string Description { get; init; } = ""!;

    public static Category Create(short code, string name, string description)
    {
        return new Category(Guid.NewGuid(), code, name, description);
    }

    public static Category Load(Guid id, short code, string name, string description)
    {
        return new Category(id, code, name, description);
    }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrDefault(Id, CategoryInvalidId);
        AssertionConcern.ValidateLessThanEqualToMinimum(Code, 0, CodeLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Name, NameNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Name, NameMaxLenght, NameMaxLenghtError);
        AssertionConcern.ValidateNullOrEmpty(Description, DescriptionNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Description, DescriptionMaxLenght, DescriptionMaxLenghtError);
    }
}
