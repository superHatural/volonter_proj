using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Species;

namespace VolunteerProg.Domain.Volunteers;

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