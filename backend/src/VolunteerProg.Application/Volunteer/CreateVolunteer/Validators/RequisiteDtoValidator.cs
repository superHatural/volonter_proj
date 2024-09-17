using FluentValidation;
using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer.Validators;

public class RequisiteDtoValidator : AbstractValidator<RequisiteDto>
{
    public RequisiteDtoValidator()
    {
        RuleFor(r => r.Description).NotEmpty().MaximumLength(500);
        RuleFor(r => r.Title).NotEmpty().MaximumLength(50);
    }
}