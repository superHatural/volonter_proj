using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record Email
{
    public string EmailAddress { get; } = default!;
    private Email(string emailAddress)
    {
        EmailAddress = emailAddress;
    }

    public static Result<Email,Error> Create(string email)
    {
        var pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        if (string.IsNullOrEmpty(email))
            return Errors.General.ValueIsRequired("EmailAddress");
        if (Regex.Match(email, pattern).Success)
            return new Email(email);
        else
            return Errors.General.ValueIsInvalid("EmailAddress");
    }
}