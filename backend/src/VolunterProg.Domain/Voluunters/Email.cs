using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record Email
{
    public string EmailAddress { get; } = default!;
    private Email(string emailAddress)
    {
        EmailAddress = emailAddress;
    }

    public static Result<Email> Create(string email)
    {
        var pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        if (Regex.Match(email, pattern).Success)
            return Result.Success(new Email(email));
        else
            return Result.Failure<Email>("Invalid email address");
    }
}