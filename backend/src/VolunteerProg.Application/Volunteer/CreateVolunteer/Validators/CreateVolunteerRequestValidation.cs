using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.CreateVolunteer.Requests;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer.Validators;

public class CreateVolunteerRequestValidation : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidation()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName));
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Description).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.Experience).GreaterThan(13).WithError(Errors.General.ValueIsInvalid("experience"));
        RuleFor(c => c.PhoneNumber).MustBeValueObject(Phone.Create);
        RuleForEach(c => c.RequisitesRecords)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));
        RuleForEach(c => c.SocialMediaRecords)
            .MustBeValueObject(x => SocialMedia.Create(x.Title, x.Link));
    }
}