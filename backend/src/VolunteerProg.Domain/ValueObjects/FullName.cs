using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.ValueObjects;

public record FullName
{
    public string FirstName { get; } = default!;
    public string LastName { get;  } = default!;
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<FullName, Error> Create(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            return Errors.General.ValueIsRequired("firstName");
        if (string.IsNullOrEmpty(lastName))
            return Errors.General.ValueIsRequired("lastName");
        return new FullName(firstName, lastName);
    }
}