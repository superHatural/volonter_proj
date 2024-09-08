using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record PetPhoto
{
    public string Path { get;  } = default!;
    public bool IsMainImage { get;  } = default!;

    private PetPhoto(string path, bool isMainImage)
    {
        Path = path;
        IsMainImage = isMainImage;
    }

    public static Result<PetPhoto> Create(string path, bool isMainImage)
    {
        if (string.IsNullOrEmpty(path))
            return Result.Failure<PetPhoto>($"Path is required.");
        return Result.Success(new PetPhoto(path, isMainImage));
    }
}