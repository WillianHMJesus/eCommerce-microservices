using WH.SharedKernel;

namespace EM.Authentication.Domain.Entities;

public sealed class Profile : BaseEntity
{
    public const int NameMaxLenght = 50;
    public const string NameNullOrEmpty = "The profile name cannot be null or empty";
    public const string ProfileNameNullOrEmpty = "The profile name cannot be null or empty";
    public const string ProfileNotFound = "The user profile not found";
    public static readonly string NameMaxLenghtError = $"The profile name cannot be greater than {NameMaxLenght}";

    public Profile(string name)
    {
        Name = name;
        Users = new List<User>();
        Roles = new List<Role>();

        Validate();
    }

    public string Name { get; init; }
    public IList<User> Users { get; private set; }
    public IList<Role> Roles { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrEmpty(Name, NameNullOrEmpty);
        AssertionConcern.ValidateMaxLength(Name, NameMaxLenght, NameMaxLenghtError);
    }
}
