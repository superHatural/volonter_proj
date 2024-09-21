using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia.Requests;

public record UpdateVolunteerSocialMediaRequest(Guid VolunteerId, UpdateVolunteerSocialMediaDto Dto);