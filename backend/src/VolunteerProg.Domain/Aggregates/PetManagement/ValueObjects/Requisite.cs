using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record Requisite
{
    public string Title { get; } = default!;
    public string Description { get; } = default!;

    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public static Result<Requisite, Error> Create(string title, string description)
    {
        if (string.IsNullOrEmpty(title))
            return Errors.General.ValueIsRequired("Title");
        if (string.IsNullOrEmpty(description))
            return Errors.General.ValueIsRequired("Description");
        return new Requisite(title, description);
    }
}