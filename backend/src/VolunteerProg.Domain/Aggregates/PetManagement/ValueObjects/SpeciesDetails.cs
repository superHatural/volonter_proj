using CSharpFunctionalExtensions;
using VolunteerProg.Domain.PetManagement.ValueObjects.Ids;

namespace VolunteerProg.Domain.PetManagement.ValueObjects;

public record SpeciesDetails
{
    private SpeciesDetails(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }
    public Guid BreedId { get; }

    public static Result<SpeciesDetails> Create(SpeciesId speciesId, BreedId breedId)
    {
        return Result.Success(new SpeciesDetails(speciesId, breedId));
    }
}