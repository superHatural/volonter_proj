using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Ids;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.ValueObjects;
using VolunteerProg.Domain.Volunteers;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullNameResult = FullName.Create(request.FirstName, request.LastName);
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return emailResult.Error;
        
        var volunteer = await _volunteersRepository.GetByEmail(emailResult.Value, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.AlreadyExist();

        var descriptionResult = NotEmptyVo.Create(request.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;

        int exp = request.Experience;

        var phoneResult = Phone.Create(request.PhoneNumber);
        if (phoneResult.IsFailure)
            return phoneResult.Error;
        
        volunteer = await _volunteersRepository.GetByPhoneNumber(phoneResult.Value, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.AlreadyExist();

        var requisite = request.RequisitesRecords.Select(req =>
                Requisite.Create(req.Title, req.Description).Value)
            .ToList();

        var socialMedia = request.SocialMediaRecords
            .Select(socMed =>
                SocialMedia.Create(socMed.Title, socMed.Link).Value)
            .ToList();

        

        var volunteerResult = Domain.Volunteers.Volunteer.Create(
            volunteerId,
            fullNameResult.Value,
            emailResult.Value,
            descriptionResult.Value,
            exp,
            phoneResult.Value,
            new SocialMediasDetails(socialMedia),
            new RequisiteDetails(requisite));
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return volunteerId.Value;
    }
}