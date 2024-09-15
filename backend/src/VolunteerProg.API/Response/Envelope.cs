using VolunteerProg.Domain.Shared;

namespace VolunteerProg.API.Response;

public record Envelope
{
    public object? Result { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }

    public DateTime CreatedTime { get; }

    public Envelope(object? result, Error? error)
    {
        Result = result;
        ErrorCode = error?.Code;
        ErrorMessage = error?.Message;
        CreatedTime = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new Envelope(result, null);

    public static Envelope Error(Error error) =>
        new Envelope(null, error);
}