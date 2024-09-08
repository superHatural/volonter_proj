namespace VolunterProg.Domain.Voluunters;

public record PetDetails
{
    public List<PetPhoto> PetPhotos { get; private set; }
    public List<Requisite> Requisites { get; private set; } 
}