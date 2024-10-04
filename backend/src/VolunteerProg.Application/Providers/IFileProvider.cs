using CSharpFunctionalExtensions;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Providers;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken);

    public Task<Result<IReadOnlyList<string>, ErrorList>> GetFiles(
        IEnumerable<string> fileNames,
        CancellationToken cancellationToken);

    public  Task<UnitResult<ErrorList>> DeleteFile(
        IEnumerable<string> fileNames,
        CancellationToken cancellationToken);

    public Task<UnitResult<ErrorList>> RemoveFile(FileInformation fileInformation,
        CancellationToken cancellationToken);
}