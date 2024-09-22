using CSharpFunctionalExtensions;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Providers;

public interface IFileProvider
{
    public Task<Result<string, Error>> UploadFile(
        FileData fileData,
        CancellationToken cancellationToken);

    public Task<Result<List<string>, Error>> GetFiles(
        List<string> fileNames,
        CancellationToken cancellationToken);

    public Task<Result<string, Error>> DeleteFile(
        string fileName,
        CancellationToken cancellationToken);
}