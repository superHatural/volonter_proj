namespace VolunterProg.Domain.Voluunters;

public record VoluunterDetails
{
    public List<Requisite> Requisites { get; private set; }
    public List<SocialMedia> SocialMedias { get; private set; }
}