namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record PetPhotoDetails
{
    public IReadOnlyList<FilePathData> PetPhotos;

    private PetPhotoDetails()
    {
    }

    public PetPhotoDetails(IEnumerable<FilePathData> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }
}