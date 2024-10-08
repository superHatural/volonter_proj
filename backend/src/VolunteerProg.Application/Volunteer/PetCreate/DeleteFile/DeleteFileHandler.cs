using CSharpFunctionalExtensions;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.PetCreate.DeleteFile;

public class DeleteFileHandler
{
    private readonly IFileProvider _fileProvider;

    public DeleteFileHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(IEnumerable<string> request, CancellationToken cancellationToken)
    {
        var result = await _fileProvider.DeleteFile(request, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        return Result.Success<ErrorList>();
    }
}