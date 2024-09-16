using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.Volunteers;

public record PetPhoto
{
    public string Path { get; } = default!;
    public bool IsMainImage { get; } = default!;

    private PetPhoto(string path, bool isMainImage)
    {
        Path = path;
        IsMainImage = isMainImage;
    }

    public static Result<PetPhoto, Error> Create(string path, bool isMainImage)
    {
        if (string.IsNullOrEmpty(path))
            return Errors.General.ValueIsRequired("Path");
        return new PetPhoto(path, isMainImage);
    }
}