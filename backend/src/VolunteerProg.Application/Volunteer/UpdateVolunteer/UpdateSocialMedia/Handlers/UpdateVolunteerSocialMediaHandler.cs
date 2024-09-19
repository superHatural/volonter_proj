using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Requests;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Handlers;

public class UpdateVolunteerSocialMediaHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerSocialMediaHandler> _logger;

    public UpdateVolunteerSocialMediaHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerSocialMediaHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerSocialMediaRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (!volunteer.IsSuccess)
            return Errors.General.NotFound(request.VolunteerId);

        var socialMedia = request.Dto.SocialMediaRecords
            .Select(sm => SocialMedia.Create(sm.Title, sm.Link).Value)
            .ToList();

        var volunteerResult = volunteer.Value.UpdateSocialMediaInfo(
            new SocialMediasDetails(socialMedia));
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        await _volunteersRepository.Update(volunteerResult.Value, cancellationToken);
        _logger.LogInformation("Updated volunteer with id: {volunteerId}", (Guid)volunteerId);

        return volunteerId.Value;
    }
}