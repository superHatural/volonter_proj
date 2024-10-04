using Microsoft.Extensions.Logging;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Messaging;
using VolunteerProg.Application.Providers;

namespace VolunteerProg.Infrastructure.Files;

public class FilesCleanerService : IFilesCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<FilesCleanerService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInformation>> _messageQueue;

    public FilesCleanerService(IMessageQueue<IEnumerable<FileInformation>> messageQueue,
        ILogger<FilesCleanerService> logger, 
        IFileProvider fileProvider)
    {
        _messageQueue = messageQueue;
        _logger = logger;
        _fileProvider = fileProvider;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var file in fileInfos)
        {
            await _fileProvider.RemoveFile(file, cancellationToken);
        }
    }
}