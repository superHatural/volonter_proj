using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record Date
{
    public string DateTime { get; } = default!;
    private Date(string dateTime)
    {
        DateTime = dateTime;
    }

    public static Result<Date,Error> Create(string dateTime)
    {
        if (string.IsNullOrEmpty(dateTime))
            return Errors.General.ValueIsRequired("Date");
        if (!System.DateTime.TryParse(dateTime, out var time))
            return Errors.General.ValueIsInvalid("Date");
        return new Date(dateTime);
    }
}