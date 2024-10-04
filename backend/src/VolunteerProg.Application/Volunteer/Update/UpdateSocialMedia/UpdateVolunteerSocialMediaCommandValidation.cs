using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia;

public class UpdateVolunteerSocialMediaCommandValidation : AbstractValidator<UpdateVolunteerSocialMediaCommand>
{
    public UpdateVolunteerSocialMediaCommandValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto).SetValidator(new UpdateVolunteerSocialMediaDtoValidation());

    }
}