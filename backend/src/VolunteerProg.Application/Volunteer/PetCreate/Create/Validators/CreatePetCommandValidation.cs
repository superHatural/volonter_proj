using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.PetCreate.Create.Requests;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.PetCreate.Create.Validators;

public class CreatePetCommandValidation : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Name).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.Description).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.Species).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Breed).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Color).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.HealthInfo).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.Address)
            .SetValidator(new AddressDtoValidation())
            .MustBeValueObject(x =>Address.Create(x.City, x.Country, x.PostalCode, x.Street));
        RuleFor(c => c.Weight).GreaterThan(0).WithError(Errors.General.ValueIsInvalid("weight"));
        RuleFor(c => c.Height).GreaterThan(0).WithError(Errors.General.ValueIsInvalid("height"));
        RuleFor(c => c.Phone).MustBeValueObject(Phone.Create);
        RuleFor(c => c.BirthDate).MustBeValueObject(Date.Create);
        RuleForEach(c => c.RequisitesRecords)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));
        
    }
}