using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Delete.Requests;

public class DeleteVolunteerValidation : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidation()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());;
    }
}