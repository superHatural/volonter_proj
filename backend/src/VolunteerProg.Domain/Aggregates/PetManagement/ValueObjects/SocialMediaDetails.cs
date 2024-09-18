namespace VolunteerProg.Domain.PetManagement.ValueObjects;

public record SocialMediasDetails
{
    public IReadOnlyList<SocialMedia> SocialMedias;

    private SocialMediasDetails()
    {
    }

    public SocialMediasDetails(IEnumerable<SocialMedia> socialMedias)
    {
        SocialMedias = socialMedias.ToList();
    }
}