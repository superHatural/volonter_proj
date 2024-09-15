using VolunteerProg.Domain.ValueObjects;

namespace VolunteerProg.Domain.Volunteers;

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