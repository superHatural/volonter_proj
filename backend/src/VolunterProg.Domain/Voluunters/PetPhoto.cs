using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record PetPhoto
{
    public string Path { get; private set; } = default!;
    public bool IsMainImage { get; private set; } = default!;

    private PetPhoto(string path, bool isMainImage)
    {
        Path = path;
        IsMainImage = isMainImage;
    }

    public static Result<PetPhoto> Create(string path, bool isMainImage)
    {
        if (string.IsNullOrEmpty(path))
            return Result.Failure<PetPhoto>($"Path is required.");
        var petPhoto = new PetPhoto(path, isMainImage);
        return Result.Success(petPhoto);
    }
}