namespace EM.Common.Core.Domain;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
        Active = true;
    }

    public Guid Id { get; init; }
    public bool Active { get; set; }

    public abstract void Validate();

    public void Inactivate() => Active = false;
}
