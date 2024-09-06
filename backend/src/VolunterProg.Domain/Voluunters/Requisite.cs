using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record Requisite
{
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;

    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }
    public static Result<Requisite> Create(string title, string description)
    {
        if (string.IsNullOrEmpty(title))
            return Result.Failure<Requisite>($"Title is required.");
        if (string.IsNullOrEmpty(description))
            return Result.Failure<Requisite>($"Description is required.");
        var requisite = new Requisite(title, description);
        return Result.Success(requisite);
    }
}