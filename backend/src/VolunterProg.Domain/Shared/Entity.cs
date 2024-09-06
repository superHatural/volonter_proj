namespace VolunterProg.Domain.Shared;

public abstract class Entity<TID> where TID: notnull
{
    protected Entity(TID id) => Id = id;
    public TID Id { get; private set; }
}