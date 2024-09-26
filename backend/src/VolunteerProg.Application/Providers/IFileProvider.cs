using CSharpFunctionalExtensions;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Providers;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken);

    public Task<Result<IReadOnlyList<string>, Error>> GetFiles(
        IEnumerable<string> fileNames,
        CancellationToken cancellationToken);

    public  Task<UnitResult<Error>> DeleteFile(
        IEnumerable<string> fileNames,
        CancellationToken cancellationToken);
}