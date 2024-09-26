using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record FilePath
{
    private static readonly string[] extensions = { ".png", ".jpg", ".jpeg", ".mp4", ".mov", ".wmv" };

    private FilePath()
    {
    }

    private FilePath(NotEmptyVo path)
    {
        Path = path;
    }
    private FilePath(NotEmptyVo path, string extension)
    {
        Path = path;
        Extension = extension;
    }

    public NotEmptyVo Path { get; }
    public string Extension { get; private set; }

    public static Result<FilePath, Error> Create(NotEmptyVo path, string extension)
    {
        foreach (var ext in extensions)
        {
            if (ext == extension)
            {
                var newPath = path.Value + extension;
                return new FilePath(NotEmptyVo.Create(newPath).Value, extension);
            }
        }

        return Errors.General.ValueIsInvalid("Extension");
    }

    public static Result<FilePath, Error> Create(NotEmptyVo path)
    {
        return new FilePath(NotEmptyVo.Create(path.Value).Value);
    }

}
