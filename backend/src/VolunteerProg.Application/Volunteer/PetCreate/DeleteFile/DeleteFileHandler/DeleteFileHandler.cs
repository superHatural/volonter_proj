using CSharpFunctionalExtensions;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.PetCreate.DeleteFile.DeleteFileHandler;

public class DeleteFileHandler
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(string request, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.DeleteFile(request, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        return result;
    }
}