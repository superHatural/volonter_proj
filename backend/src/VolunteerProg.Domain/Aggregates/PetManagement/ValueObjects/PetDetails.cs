namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record PetPhotoDetails
{
    public IReadOnlyList<PetPhoto> PetPhotos;

    private PetPhotoDetails()
    {
    }

    public PetPhotoDetails(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }
}