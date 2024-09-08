using System.Threading.Tasks.Sources;
using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record NotEmptyVo
{
    public string Value { get;} = default!;
    private NotEmptyVo(string value)
    {
        Value = value;
    }

    public static Result<NotEmptyVo> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<NotEmptyVo>($"Cannot be null or empty.");
        return Result.Success(new NotEmptyVo(value));
    }
}