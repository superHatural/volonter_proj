using System.Threading.Tasks.Sources;
using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record NotEmptyVo
{
    public string Value { get;} = default!;
    private NotEmptyVo(string value)
    {
        Value = value;
    }

    public static Result<NotEmptyVo, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Errors.General.ValueIsRequired("lastName");
        return new NotEmptyVo(value);
    }
}