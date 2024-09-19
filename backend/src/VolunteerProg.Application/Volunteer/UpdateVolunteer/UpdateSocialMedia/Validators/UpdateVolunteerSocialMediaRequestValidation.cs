using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Requests;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Validators;

public class UpdateVolunteerSocialMediaRequestValidation : AbstractValidator<UpdateVolunteerSocialMediaRequest>
{
    public UpdateVolunteerSocialMediaRequestValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto).SetValidator(new UpdateVolunteerSocialMediaDtoValidation());

    }
}