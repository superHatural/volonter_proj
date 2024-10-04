using VolunteerProg.Domain.Shared;

namespace VolunteerProg.API.Response;

public record ResponseError (string? ErrorCode, string? ErrorMessage, string? InvalidField);
public record Envelope
{
    public object? Result { get; }

    public ErrorList Errors { get; }

    public DateTime CreatedTime { get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        CreatedTime = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new (result, null);

    public static Envelope Error(ErrorList errors) =>
        new (null, errors);
}