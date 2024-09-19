using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Requests;

public record UpdateVolunteerSocialMediaRequest(Guid VolunteerId, UpdateVolunteerSocialMediaDto Dto);