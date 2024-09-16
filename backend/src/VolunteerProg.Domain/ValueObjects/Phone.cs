using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.ValueObjects;

public record Phone
{
    public string PhoneNumber { get; } = default!;
    private const string PhoneRegex = @"^(\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$";

    private Phone(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public static Result<Phone, Error> Create(string input)
    {
        var phoneNumber = input?.Trim();

        if (string.IsNullOrEmpty(phoneNumber))
            return Errors.General.ValueIsRequired("phoneNumber");

        if (!Regex.Match(phoneNumber, PhoneRegex).Success)
            return Errors.General.ValueIsInvalid("Invalid phone number");

        return new Phone(phoneNumber);
    }
}