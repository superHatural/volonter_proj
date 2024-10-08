using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia;

public class UpdateVolunteerSocialMediaHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerSocialMediaHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerSocialMediaHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerSocialMediaHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerSocialMediaCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (!volunteer.IsSuccess)
                return Errors.General.NotFound(command.VolunteerId);

            var socialMedia = command.Dto.SocialMediaRecords
                .Select(sm => SocialMedia.Create(sm.Title, sm.Link).Value)
                .ToList();

            var volunteerResult = volunteer.Value.UpdateSocialMediaInfo(
                new ValueObjectList<SocialMedia>(socialMedia));
            if (volunteerResult.IsFailure)
                return volunteerResult.Error;

            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("Updated volunteer with id: {volunteerId}", (Guid)volunteerId);

            return volunteerId.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot update social media in volunteer - {id} in transaction", command.VolunteerId);

            transaction.Rollback();
            return Error.Failure("Cannot update social media in volunteer", "volunteer.social.media.failure");
        }
    }
}