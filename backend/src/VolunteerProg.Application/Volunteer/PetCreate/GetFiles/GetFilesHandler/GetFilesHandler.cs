using CSharpFunctionalExtensions;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.PetCreate.GetFiles.GetFilesHandler;

public class GetFilesHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFilesHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<List<string>, Error>> Handle(List<string> request,
        CancellationToken cancellationToken)
    {
        var result = await _fileProvider.GetFiles(request, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        return result;
    }
}