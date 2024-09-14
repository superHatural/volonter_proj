using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record VoluunterDetails
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<SocialMedia> _socialMedia = [];
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<SocialMedia> SocialMedias => _socialMedia;
    private VoluunterDetails() { }
    private VoluunterDetails(Requisite requisite, SocialMedia socialMedia)
    {
        _requisites.Add(requisite);
        _socialMedia.Add(socialMedia);
    }

    public static Result<VoluunterDetails, Error> Create(string reqTitle, string reqDescription, string socMedTitle,
        string socMedUrl)
    {
        return new VoluunterDetails(
            Requisite.Create(reqTitle, reqDescription).Value,
            SocialMedia.Create(socMedTitle, socMedUrl).Value); 
    }
}