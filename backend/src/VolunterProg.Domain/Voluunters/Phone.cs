using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record Phone
{
    public string PhoneNumber { get; } = default!;
    private Phone(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public static Result<Phone> Create(string phoneNumber)
    {
        var pattern = @"((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}";
        if (Regex.Match(phoneNumber, pattern).Success)
        {
            return Result.Success(new Phone(phoneNumber));
        }
        else
        {
            return Result.Failure<Phone>("Invalid phone number");
        }
        

    }
}