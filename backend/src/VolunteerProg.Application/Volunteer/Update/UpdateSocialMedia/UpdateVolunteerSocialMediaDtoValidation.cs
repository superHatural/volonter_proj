using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

namespace VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia;

public class UpdateVolunteerSocialMediaDtoValidation : AbstractValidator<UpdateVolunteerSocialMediaDto>
{
    public UpdateVolunteerSocialMediaDtoValidation()
    {
        RuleForEach(c => c.SocialMediaRecords)
            .MustBeValueObject(x => SocialMedia.Create(x.Title, x.Link));
    }
}