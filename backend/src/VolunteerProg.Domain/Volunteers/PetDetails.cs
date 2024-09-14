namespace VolunteerProg.Domain.Volunteers;

public record PetDetails
{
    public IReadOnlyList<PetPhoto> PetPhotos ;
    public IReadOnlyList<Requisite> Requisites ;
    private PetDetails() { }
    public PetDetails(IEnumerable<PetPhoto> petPhotos, IEnumerable<Requisite> requisites)
    {
        PetPhotos = petPhotos.ToList();
        Requisites = requisites.ToList();
    }
}