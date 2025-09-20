using WH.SharedKernel;

namespace EM.Authentication.Domain.Entities;

public sealed class Role : BaseEntity
{
    public const int NameMaxLenght = 50;
    public const string NameNullOrEmpty = "The role name cannot be null or empty";
    public static readonly string NameMaxLenghtError = $"The role name cannot be greater than {NameMaxLenght}";

    public Role(string name)
    {
        Name = name;
        Profiles = new List<Profile>();

        Validate();
    }

    public string Name { get; init; }
    public IList<Profile> Profiles { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrEmpty(Name, NameNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Name, NameMaxLenght, NameMaxLenghtError);
    }
}
