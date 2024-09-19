using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateRequisites.Requests;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateRequisites.Validators;

public class UpdateVolunteerRequisitesRequestValidation : AbstractValidator<UpdateVolunteerRequisitesRequest>
{
    public UpdateVolunteerRequisitesRequestValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto).SetValidator(new UpdateVolunteerRequisiteDtoValidation());
    }
}