using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record SocialMedia
{
    public string Title { get;  } = default!;
    public string Url { get; } = default!;
    private SocialMedia(string title, string url)
    {
        Title = title;
        Url = url;
    }
    public static Result<SocialMedia, Error> Create(string title, string url)
    {
        if (string.IsNullOrEmpty(title))
            return Errors.General.ValueIsRequired("Title");
        if (string.IsNullOrEmpty(url))
            return Errors.General.ValueIsRequired("Url");
        return new SocialMedia(title, url);
    }
}