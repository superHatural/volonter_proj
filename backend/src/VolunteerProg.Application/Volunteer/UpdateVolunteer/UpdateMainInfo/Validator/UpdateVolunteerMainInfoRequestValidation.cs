using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Request;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Validator;

public class UpdateVolunteerMainInfoRequestValidation : AbstractValidator<UpdateVolunteerMainInfoRequest>
{
    public UpdateVolunteerMainInfoRequestValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto)
            .SetValidator(new UpdateVolunteerMainInfoDtoValidation());
    }
}
