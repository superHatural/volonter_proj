using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Aggregates.SpeciesManagement.Entity;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Domain.Aggregates.SpeciesManagement.AggregateRoot;

public class Species : Shared.Entity<SpeciesId>
{
    public Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, NotEmptyVo title) : base(id)
    {
        Id = id;
        Title = title;
    }

    public SpeciesId Id { get; private set; } = default!;
    public NotEmptyVo Title { get; private set; } = default!;
    private readonly List<Breed> _breeds = [];
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species> Create(SpeciesId breedId, NotEmptyVo title)
    {
        return Result.Success(new Species(breedId, title));
    }
}