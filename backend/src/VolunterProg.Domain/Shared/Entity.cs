namespace VolunterProg.Domain.Shared;

public abstract class Entity<TId> where TId: notnull
{
    protected Entity(TId id) => Id = id;

    protected Entity()
    {
        throw new NotImplementedException();
    }

    public TId Id { get; private set; }
}