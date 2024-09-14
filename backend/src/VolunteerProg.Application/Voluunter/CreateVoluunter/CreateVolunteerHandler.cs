using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Volunteers;

namespace VolunteerProg.Application.Voluunter.CreateVoluunter;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    public async Task<Result<Guid,Error>> Handle(
        CreateVolunteerRequest request, 
        CancellationToken cancellationToken)
    {
        var voluunterId = VolunteerId.NewValuunterId();
        
        var fullNameResult = FullName.Create(request.FirstName, request.LastName );
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;
        
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return emailResult.Error;
        
        var descriptionResult = NotEmptyVo.Create(request.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;
        
        int exp = request.Experience;
        
        var phoneResult = Phone.Create(request.PhoneNumber);
        if (phoneResult.IsFailure)
            return phoneResult.Error;
        var voluunterDetailsResult = new VolunteerDetails(
            new List<Requisite>
                {Requisite.Create(request.RequisiteTitle, request.RequisiteDescription).Value},
            new List<SocialMedia> 
                {SocialMedia.Create(request.SocMedTitle, request.SocMedUrl).Value});


        
        var voluunterResult = Volunteer.Create(
            voluunterId, 
            fullNameResult.Value, 
            emailResult.Value,
            descriptionResult.Value,
            exp,
            phoneResult.Value, 
            voluunterDetailsResult);
        if (voluunterResult.IsFailure)
            return voluunterResult.Error;
        
        await _volunteersRepository.Add(voluunterResult.Value, cancellationToken);
        
        return voluunterId.Value;
    }
}