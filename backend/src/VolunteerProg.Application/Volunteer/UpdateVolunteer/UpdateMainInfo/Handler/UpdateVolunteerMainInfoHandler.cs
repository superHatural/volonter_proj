using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Request;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Handler;

public class UpdateVolunteerMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;

    public UpdateVolunteerMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateVolunteerMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateVolunteerMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (!volunteer.IsSuccess)
            return Errors.General.NotFound(request.VolunteerId);

        var fullName = FullName.Create(request.Dto.FullName.FirstName,
            request.Dto.FullName.LastName).Value;

        var email = Email.Create(request.Dto.Email).Value;

        volunteer = await _volunteersRepository.GetByEmail(email, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.AlreadyExist();

        var description = NotEmptyVo.Create(request.Dto.Description).Value;

        int exp = request.Dto.Experience;

        var phone = Phone.Create(request.Dto.PhoneNumber).Value;

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

        await _volunteersRepository.Update(volunteerResult.Value, cancellationToken);
        _logger.LogInformation("Updated volunteer with id: {volunteerId}", (Guid)volunteerId);

        return volunteerId.Value;
    }
}