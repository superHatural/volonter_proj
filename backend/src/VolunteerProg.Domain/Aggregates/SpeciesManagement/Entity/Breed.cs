using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Domain.Aggregates.SpeciesManagement.Entity;

public class Breed : Shared.Entity<BreedId>
{
    public Breed(BreedId id) : base(id)
    {
    }

    private Breed(BreedId id, NotEmptyVo title) : base(id)
    {
        Id = id;
        Title = title;
    }

    public NotEmptyVo Title { get; private set; }
    public BreedId Id { get; private set; }

    public static Result<Breed> Create(BreedId breedId, NotEmptyVo title)
    {
        return Result.Success(new Breed(breedId, title));
    }
}