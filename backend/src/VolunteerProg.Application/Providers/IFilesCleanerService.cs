namespace VolunteerProg.Application.Providers;

public interface IFilesCleanerService
{
    public Task Process(CancellationToken cancellationToken);
}