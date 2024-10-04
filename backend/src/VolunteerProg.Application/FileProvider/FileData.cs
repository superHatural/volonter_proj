using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

namespace VolunteerProg.Application.FileProvider;

public record FileData(Stream Stream, string BucketName, FilePath FilePath);
public record CreateFileData(Stream Stream, string FileName);