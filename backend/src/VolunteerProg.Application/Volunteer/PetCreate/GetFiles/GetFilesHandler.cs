using CSharpFunctionalExtensions;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.PetCreate.GetFiles;

public class GetFilesHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFilesHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<IReadOnlyList<string>, ErrorList>> Handle(IEnumerable<string> request,
        CancellationToken cancellationToken)
    {
        var result = await _fileProvider.GetFiles(request, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        return result;
    }
}