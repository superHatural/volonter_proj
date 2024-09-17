using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Domain.PetManagement.ValueObjects;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer.Validators;

public class CreateVolunteerRequestValidation : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidation()
    {
        RuleFor(c => new { c.FirstName, c.LastName })
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName));
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Description).MustBeValueObject(NotEmptyVo.Create);
        RuleFor(c => c.Experience).NotNull();
        RuleFor(c => c.PhoneNumber).MustBeValueObject(Phone.Create);
        RuleForEach(c => c.RequisitesRecords).SetValidator(new RequisiteDtoValidator())
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));
        RuleForEach(c => c.SocialMediaRecords).SetValidator(new SocialMediaDtoValidator())
            .MustBeValueObject(x => SocialMedia.Create(x.Title, x.Link));
    }
}