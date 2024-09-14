namespace VolunteerProg.Domain.Species;

public record BreedId
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }
    public static BreedId NewSpeciesId() => new(Guid.NewGuid());
    public static BreedId Empty() => new(Guid.Empty);
    public static BreedId Create(Guid id) => new BreedId(id);
    public static implicit operator Guid(BreedId breedId) => breedId.Value;
}