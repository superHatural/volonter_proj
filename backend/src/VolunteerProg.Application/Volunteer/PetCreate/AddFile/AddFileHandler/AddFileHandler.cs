using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Providers;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileHandler;

public class AddFileHandler
{
    private readonly ILogger<AddFileHandler> _logger;
    private readonly IFileProvider _fileProvider;

    public AddFileHandler(IFileProvider fileProvider, ILogger<AddFileHandler> logger)
    {
        _logger = logger;
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(AddFileRequest.AddFileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _fileProvider.UploadFile(
            new FileData(request.Stream, request.BucketName, request.ObjectName),
            cancellationToken);
        if (result.IsFailure)
            return result.Error;
        return result.Value;
    }
}