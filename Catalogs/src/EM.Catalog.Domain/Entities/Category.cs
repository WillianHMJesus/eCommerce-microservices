namespace EM.Catalog.Domain.Entities;

public class Category : Entity
{
    public const string ErrorMessageCodeLessThanEqualToZero = "The category code cannot be less than or equal to zero.";
    public const string ErrorMessageNameNullOrEmpty = "The category name cannot be null or empty.";
    public const string ErrorMessageDescriptionNullOrEmpty = "The category description cannot be null or empty.";

    public Category(short code, string name, string description)
    {
        Code = code;
        Name = name;
        Description = description;

        Validate();
    }

    public short Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public override void Validate()
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(Code, 0, ErrorMessageCodeLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Name, ErrorMessageNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(Description, ErrorMessageDescriptionNullOrEmpty);
    }
}
