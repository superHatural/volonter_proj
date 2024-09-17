using FluentValidation;
using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer.Validators;

public class SocialMediaDtoValidator : AbstractValidator<SocialMediaDto>
{
    public SocialMediaDtoValidator()
    {
        RuleFor(r => r.Link).NotEmpty().MaximumLength(200);
        RuleFor(r => r.Title).NotEmpty().MaximumLength(50);
    }
}