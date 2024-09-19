using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record Email
{
    public string EmailAddress { get; } = default!;
    public const string EmailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

    private Email(string emailAddress)
    {
        EmailAddress = emailAddress;
    }

    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrEmpty(email))
            return Errors.General.ValueIsRequired("EmailAddress");
        if (!Regex.Match(email, EmailRegex).Success)
            return Errors.General.ValueIsInvalid("EmailAddress");

        return new Email(email);
    }
}