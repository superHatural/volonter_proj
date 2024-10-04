using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Update.UpdateMainInfo;

public class UpdateVolunteerMainInfoCommandValidation : AbstractValidator<UpdateVolunteerMainInfoCommand>
{
    public UpdateVolunteerMainInfoCommandValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto)
            .SetValidator(new UpdateVolunteerMainInfoDtoValidation());
    }
}
