using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Volunteer.Delete.Requests;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Delete.Handlers;

public class DeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (!volunteer.IsSuccess)
            return Errors.General.NotFound(request.VolunteerId);

        volunteer.Value.Delete();
        
        await _volunteersRepository.Save(volunteer.Value, cancellationToken);
        _logger.LogInformation("Deleted volunteer with id: {volunteerId}", (Guid)volunteerId);

        return volunteerId.Value;
    }
}