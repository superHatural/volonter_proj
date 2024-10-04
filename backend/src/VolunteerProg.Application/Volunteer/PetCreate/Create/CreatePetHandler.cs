using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.Extensions;
using VolunteerProg.Domain.Aggregates.PetManagement.Entities;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.PetCreate.Create;

public class CreatePetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatePetCommand> _validator;

    public CreatePetHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreatePetHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<CreatePetCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return validationResult.ToErrorList();
            }

            var volunteerResult =
                await _volunteersRepository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var pet = CreatePet(command);

            var result = volunteerResult.Value.AddPet(pet);
            if (result.IsFailure)
                return result.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();

            return pet.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Cannot create pet in volunteer({id}) in transaction", command.VolunteerId);

            transaction.Rollback();

            return Error.Failure("Cannot create pet", "pet.create.failure").ToErrorList();
        }
    }

    private static Pet CreatePet(CreatePetCommand command)
    {
        var petId = PetId.NewPetId();
        var name = NotEmptyVo.Create(command.Name).Value;
        var description = NotEmptyVo.Create(command.Description).Value;
        var speciesDetails = SpeciesDetails
            .Create(SpeciesId.Empty(), BreedId.Empty()).Value;
        var color = NotEmptyVo.Create(command.Color).Value;
        var healthInfo = NotEmptyVo.Create(command.HealthInfo).Value;

        var address = Address
            .Create(
                command.Address.City,
                command.Address.Country,
                command.Address.PostalCode,
                command.Address.Street).Value;

        var phone = Phone.Create(command.Phone).Value;
        var birthDate = Date.Create(command.BirthDate).Value;
        var status = Enum.Parse<PetStatus>(command.Status);
        var requisites =
            command.RequisitesRecords.Select(p => Requisite.Create(p.Title, p.Description).Value);

        var pet = new Pet(
            petId,
            name,
            description,
            speciesDetails,
            color,
            healthInfo,
            address,
            command.Weight,
            command.Height,
            phone,
            command.IsCastrated,
            birthDate,
            command.IsVaccinated,
            status,
            null,
            new ValueObjectList<Requisite>(requisites)
        );

        return pet;
    }
}