using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.Volunteer.Create.Requests;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Create.Handlers;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
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


            var volunteerResult = new Domain.Aggregates.PetManagement.AggregateRoot.Volunteer(
                volunteerId,
                fullName,
                email,
                description,
                exp,
                phone,
                new ValueObjectList<SocialMedia>(socialMedia),
                new ValueObjectList<Requisite>(requisite));

            await _volunteersRepository.Add(volunteerResult, cancellationToken);

            transaction.Commit();

            _logger.LogInformation("Created volunteer with id: {volunteerId}", (Guid)volunteerId);

            return volunteerId.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot create volunteer in transaction");

            transaction.Rollback();
            return Error.Failure("Cannot create volunteer in transaction", "volunteer.create.failure");
        }
    }
}