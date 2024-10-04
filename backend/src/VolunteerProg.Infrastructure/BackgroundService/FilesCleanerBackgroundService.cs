using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Providers;

namespace VolunteerProg.Infrastructure.BackgroundService;

public class FilesCleanerBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundService(ILogger<FilesCleanerBackgroundService> logger,
 IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is running.");

        await using var scope = _scopeFactory.CreateAsyncScope();
        
        var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();

        while (stoppingToken.IsCancellationRequested)
        {
            await filesCleanerService.Process(stoppingToken);
        }

        await Task.CompletedTask;
    }
}