using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

namespace VolunteerProg.Application.FileProvider;

public record FileData(Stream Stream, FileInformation FileInformation);
public record CreateFileData(Stream Stream, string FileName);

public record FileInformation(string BucketName, FilePath FilePath);