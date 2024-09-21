using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Update.UpdateMainInfo.Validator;

public class UpdateVolunteerMainInfoDtoValidation : AbstractValidator<UpdateVolunteerMainInfoDto>
{
    public UpdateVolunteerMainInfoDtoValidation()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName));
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Description).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.Experience).GreaterThan(13)
            .WithError(Errors.General.ValueIsInvalid("experience"));
        RuleFor(c => c.PhoneNumber).MustBeValueObject(Phone.Create);
    }
}