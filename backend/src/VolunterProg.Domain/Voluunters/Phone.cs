using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record Phone
{
    public string PhoneNumber { get; } = default!;
    private Phone(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public static Result<Phone, Error> Create(string phoneNumber)
    {
        var pattern = @"((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}";
        if (string.IsNullOrEmpty(phoneNumber))
            return Errors.General.ValueIsRequired("phoneNumber");
        if (Regex.Match(phoneNumber, pattern).Success)
        {
            return new Phone(phoneNumber);
        }
        else
        {
            return Errors.General.ValueIsInvalid("Invalid phone number");
        }
        

    }
}