using System.Runtime.InteropServices.JavaScript;

namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record FilePathData
{
    public FilePath Path { get; }
    public bool IsMainImage { get; }

    private FilePathData()
    {
        
    }
    public FilePathData(FilePath path, bool isMainImage)
    {
        Path = path;
        IsMainImage = isMainImage;
    }
    
}