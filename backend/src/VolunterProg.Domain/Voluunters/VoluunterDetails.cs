using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record VoluunterDetails
{
    public IReadOnlyList<Requisite> Requisites ;
    public IReadOnlyList<SocialMedia> SocialMedias ;
    private VoluunterDetails() { }
    public VoluunterDetails(IEnumerable<Requisite> requisites, IEnumerable<SocialMedia> socialMedias)
    {
       Requisites = requisites.ToList();
       SocialMedias = socialMedias.ToList();
    }
    
}