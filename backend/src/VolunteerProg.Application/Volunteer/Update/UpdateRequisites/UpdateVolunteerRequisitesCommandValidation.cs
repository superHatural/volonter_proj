using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Update.UpdateRequisites;

public class UpdateVolunteerRequisitesCommandValidation : AbstractValidator<UpdateVolunteerRequisitesCommand>
{
    public UpdateVolunteerRequisitesCommandValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Dto).SetValidator(new UpdateVolunteerRequisiteDtoValidation());
    }
}