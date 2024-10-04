using CSharpFunctionalExtensions;

namespace VolunteerProg.Domain.Shared;

public record NotEmptyVo
{
    public string Value { get; } = default!;
    
    private NotEmptyVo(string value)
    {
        Value = value;
    }

    public static Result<NotEmptyVo, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Errors.General.ValueIsRequired("value");
        return new NotEmptyVo(value);
    }
    
}