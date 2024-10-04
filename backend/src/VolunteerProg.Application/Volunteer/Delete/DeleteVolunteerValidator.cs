using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}