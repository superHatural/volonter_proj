using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.Volunteer.Update.UpdateRequisites.Requests;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Update.UpdateRequisites.Handlers;

public class UpdateVolunteerRequisitesHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerRequisitesHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerRequisitesHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerRequisitesHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteerId = VolunteerId.Create(request.VolunteerId);
            var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (!volunteer.IsSuccess)
                return Errors.General.NotFound(request.VolunteerId);

            var requisite = request.Dto.RequisitesRecords
                .Select(req => Requisite.Create(req.Title, req.Description).Value)
                .ToList();

            var volunteerResult = volunteer.Value.UpdateRequisiteInfo(
                new RequisiteDetails(requisite));
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
                "Cannot update requisite in volunteer - {id} in transaction", request.VolunteerId);

            transaction.Rollback();
            return Error.Failure("Cannot update requisite in volunteer", "volunteer.requisite.failure");
        }

    }
}