using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Aggregates.SpeciesManagement.Entity;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Domain.Aggregates.SpeciesManagement.AggregateRoot;

public class Species : Shared.Entity<SpeciesId>, ISoftDelete
{
    public Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, NotEmptyVo title) : base(id)
    {
        Id = id;
        Title = title;
    }

    private bool _deleted = false;
    public SpeciesId Id { get; private set; } = default!;
    public NotEmptyVo Title { get; private set; } = default!;
    private readonly List<Breed> _breeds = [];
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species> Create(SpeciesId id, NotEmptyVo title)
    {
        return Result.Success(new Species(id, title));
    }

    public void Delete()
    {
        if (!_deleted)
            _deleted = true;
        foreach (var breed in Breeds)
        {
            breed.Delete();
        }
    }

    public void Restore()
    {
        if (!_deleted)
            _deleted = false;
        foreach (var breed in Breeds)
        {
            breed.Restore();
        }
    }
}