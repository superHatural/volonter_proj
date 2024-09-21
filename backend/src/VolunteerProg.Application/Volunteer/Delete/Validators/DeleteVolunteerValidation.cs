using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.Delete.Requests;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Delete.Validators;

public class DeleteVolunteerValidation : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());;
    }
}