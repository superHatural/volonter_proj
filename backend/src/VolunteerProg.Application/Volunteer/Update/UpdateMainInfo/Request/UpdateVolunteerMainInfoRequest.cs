using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.Update.UpdateMainInfo.Request;

public record UpdateVolunteerMainInfoRequest(
    Guid VolunteerId,
    UpdateVolunteerMainInfoDto Dto);