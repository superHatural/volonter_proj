using CSharpFunctionalExtensions;
using VolunteerProg.Domain.PetManagement.ValueObjects;
using VolunteerProg.Domain.PetManagement.ValueObjects.Ids;

namespace VolunteerProg.Domain.PetManagement.Entities;

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