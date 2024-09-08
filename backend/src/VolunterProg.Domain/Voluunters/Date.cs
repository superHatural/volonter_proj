using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record Date
{
    public string DateTime { get; } = default!;
    private Date(string dateTime)
    {
        DateTime = dateTime;
    }

    public static Result<Date> Create(string dateTime)
    {
        if (!System.DateTime.TryParse(dateTime, out var time) || string.IsNullOrEmpty(dateTime))
            return Result.Failure<Date>($"DateTime is invalid.");
        return Result.Success(new Date(dateTime));
    }
}