namespace VolunteerProg.Domain.Volunteers;

public record VolunteerDetails
{
    public IReadOnlyList<Requisite> Requisites ;
    public IReadOnlyList<SocialMedia> SocialMedias ;
    private VolunteerDetails() { }
    public VolunteerDetails(IEnumerable<Requisite> requisites, IEnumerable<SocialMedia> socialMedias)
    {
       Requisites = requisites.ToList();
       SocialMedias = socialMedias.ToList();
    }
    
}