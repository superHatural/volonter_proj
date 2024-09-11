namespace VolunterProg.Domain.Voluunters;

public class Breed: Shared.Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
        
    }

    public Breed(BreedId id, NotEmptyVo title) : base(id)
    {
        Id = id;
        Title = title;
    }
    public NotEmptyVo Title { get; private set; }
    public BreedId Id { get; private set; }
    
}