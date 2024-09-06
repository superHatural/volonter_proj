using CSharpFunctionalExtensions;

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
    public static Result<SocialMedia> Create(string title, string url)
    {
        if (string.IsNullOrEmpty(title))
            return Result.Failure<SocialMedia>($"Title is required.");
        if (string.IsNullOrEmpty(url))
            return Result.Failure<SocialMedia>($"Description is required.");
        return Result.Success(new SocialMedia(title, url));
    }
}