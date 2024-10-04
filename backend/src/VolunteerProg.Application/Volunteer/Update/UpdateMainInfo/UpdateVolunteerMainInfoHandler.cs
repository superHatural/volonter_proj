using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Update.UpdateMainInfo;

public class UpdateVolunteerMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVolunteerMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerMainInfoHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction();
        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (!volunteer.IsSuccess)
                return Errors.General.NotFound(command.VolunteerId);

            var fullName = FullName.Create(command.Dto.FullName.FirstName,
                command.Dto.FullName.LastName).Value;

            var email = Email.Create(command.Dto.Email).Value;

            volunteer = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteer.IsSuccess)
                return Errors.General.AlreadyExist();

            var description = NotEmptyVo.Create(command.Dto.Description).Value;

            int exp = command.Dto.Experience;

            var phone = Phone.Create(command.Dto.PhoneNumber).Value;

            volunteer = await _volunteersRepository.GetByPhoneNumber(phone, cancellationToken);
            if (volunteer.IsSuccess)
                return Errors.General.AlreadyExist();

            volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            var volunteerResult = volunteer.Value.UpdateMainInfo(
                fullName,
                email,
                description,
                exp,
                phone);
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
                "Cannot update main info in volunteer - {id} in transaction", command.VolunteerId);

            transaction.Rollback();
            return Error.Failure("Cannot update main info in volunteer", "volunteer.main.info.failure");
        }
    }
}