using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record VoluunterDetails
{
    public List<Requisite> Requisites { get;  }
    public List<SocialMedia> SocialMedias { get;  }
    private VoluunterDetails() { }
    private VoluunterDetails(string reqTitle, string reqDescription, string socMedTitle,
        string socMedUrl)
    {
        Requisites = new List<Requisite>();
        Requisites.Add(Requisite.Create(reqTitle, reqDescription).Value);
        SocialMedias = new List<SocialMedia>();
        SocialMedias.Add(SocialMedia.Create(socMedTitle, socMedUrl).Value);
    }

    public static Result<VoluunterDetails> Create(string reqTitle, string reqDescription, string socMedTitle,
        string socMedUrl)
    {
        return Result.Success(new VoluunterDetails(reqTitle, reqDescription, socMedTitle, socMedUrl)); 
    }
}