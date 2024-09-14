namespace VolunterProg.Domain.Voluunters;

public record PetDetails
{
    public List<PetPhoto> PetPhotos { get; }
    public List<Requisite> Requisites { get; } 
    
}