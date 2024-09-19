using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Requests;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Validators;

public class UpdateVolunteerSocialMediaDtoValidation : AbstractValidator<UpdateVolunteerSocialMediaDto>
{
    public UpdateVolunteerSocialMediaDtoValidation()
    {
        RuleForEach(c => c.SocialMediaRecords)
            .MustBeValueObject(x => SocialMedia.Create(x.Title, x.Link));
    }
}