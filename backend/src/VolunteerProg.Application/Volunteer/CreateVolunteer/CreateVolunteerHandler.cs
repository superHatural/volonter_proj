using CSharpFunctionalExtensions;
using FluentValidation;
using VolunteerProg.Domain.PetManagement.ValueObjects;
using VolunteerProg.Domain.PetManagement.ValueObjects.Ids;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(request.FullName.FirstName, request.FullName.LastName).Value;

        var email = Email.Create(request.Email).Value;

        var volunteer = await _volunteersRepository.GetByEmail(email, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.AlreadyExist();
        
        var description = NotEmptyVo.Create(request.Description).Value;

        int exp = request.Experience;

        var phone = Phone.Create(request.PhoneNumber).Value;

        volunteer = await _volunteersRepository.GetByPhoneNumber(phone, cancellationToken);
        if (volunteer.IsSuccess)
            return Errors.General.AlreadyExist();

        var requisite = request.RequisitesRecords
            .Select(req => Requisite.Create(req.Title, req.Description).Value)
            .ToList();
        
        var socialMedia = request.SocialMediaRecords
            .Select(socMed => SocialMedia.Create(socMed.Title, socMed.Link).Value)
            .ToList();
        


        var volunteerResult = Domain.PetManagement.AggregateRoot.Volunteer.Create(
            volunteerId,
            fullName,
            email,
            description,
            exp,
            phone,
            new SocialMediasDetails(socialMedia),
            new RequisiteDetails(requisite));
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return volunteerId.Value;
    }
}