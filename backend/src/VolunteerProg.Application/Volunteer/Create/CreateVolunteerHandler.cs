using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Create;

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
        CreateVolunteerСommand сommand,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteerId = VolunteerId.NewVolunteerId();

            var fullName = FullName.Create(сommand.FullName.FirstName, сommand.FullName.LastName).Value;

            var email = Email.Create(сommand.Email).Value;

            var volunteer = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteer.IsSuccess)
                return Errors.General.AlreadyExist();

            var description = NotEmptyVo.Create(сommand.Description).Value;

            int exp = сommand.Experience;

            var phone = Phone.Create(сommand.PhoneNumber).Value;

            volunteer = await _volunteersRepository.GetByPhoneNumber(phone, cancellationToken);
            if (volunteer.IsSuccess)
                return Errors.General.AlreadyExist();

            var requisite = сommand.RequisitesRecords
                .Select(req => Requisite.Create(req.Title, req.Description).Value)
                .ToList();

            var socialMedia = сommand.SocialMediaRecords
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