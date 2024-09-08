using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record FullName
{
    public string FirstName { get; } = default!;
    public string LastName { get;  } = default!;
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<FullName> Create(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            return Result.Failure<FullName>($"First name is required.");
        if (string.IsNullOrEmpty(lastName))
            return Result.Failure<FullName>($"Last name is required.");
        return Result.Success(new FullName(firstName, lastName));
    }
}