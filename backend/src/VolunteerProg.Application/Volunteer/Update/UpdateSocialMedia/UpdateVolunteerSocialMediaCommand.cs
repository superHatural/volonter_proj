using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia;

public record UpdateVolunteerSocialMediaCommand(Guid VolunteerId, UpdateVolunteerSocialMediaDto Dto);