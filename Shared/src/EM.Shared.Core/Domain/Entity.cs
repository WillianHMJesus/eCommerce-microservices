namespace EM.Shared.Core;

public abstract class Entity
{
    public Entity()
     => Id = Guid.NewGuid();

    public Guid Id { get; init; }

    public abstract void Validate();
}
