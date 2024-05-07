using EM.Common.Core.Domain;

namespace EM.Catalog.Domain.Entities;

public class Category : Entity
{
    public Category(short code, string name, string description)
    {
        Code = code;
        Name = name;
        Description = description;

        Validate();
    }

    public short Code { get; init; }
    public string Name { get; init; } = ""!;
    public string Description { get; init; } = ""!;

    public override void Validate()
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(Code, 0, ErrorMessage.CategoryCodeLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Name, ErrorMessage.CategoryNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(Description, ErrorMessage.CategoryDescriptionNullOrEmpty);
    }
}
