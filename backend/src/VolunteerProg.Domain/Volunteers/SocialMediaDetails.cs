using VolunteerProg.Domain.ValueObjects;

namespace VolunteerProg.Domain.Volunteers;

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