using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;
using VolunterProg.Infrastructure.Repositories;

namespace VolunterProg.Application.Voluunter;

public class CreateVoluunterHandler
{
    private readonly IVoluuntersRepository _voluuntersRepository;

    public CreateVoluunterHandler(IVoluuntersRepository voluuntersRepository)
    {
        _voluuntersRepository = voluuntersRepository;
    }
    public async Task<Result<Guid,Error>> Handle(
        CreateVoluunterRequest request, 
        CancellationToken cancellationToken)
    {
        var voluunterId = VoluunterId.NewValuunterId();
        
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
        var voluunterDetailsResult = new VoluunterDetails(
            new List<Requisite>
                {Requisite.Create(request.RequisiteTitle, request.RequisiteDescription).Value},
            new List<SocialMedia> 
                {SocialMedia.Create(request.SocMedTitle, request.SocMedUrl).Value});


        
        var voluunterResult = Domain.Voluunters.Voluunter.Create(
            voluunterId, 
            fullNameResult.Value, 
            emailResult.Value,
            descriptionResult.Value,
            exp,
            phoneResult.Value, 
            voluunterDetailsResult);
        if (voluunterResult.IsFailure)
            return voluunterResult.Error;
        
        await _voluuntersRepository.Add(voluunterResult.Value, cancellationToken);
        
        return voluunterId.Value;
    }
}