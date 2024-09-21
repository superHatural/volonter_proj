using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.Update.UpdateRequisites.Requests;

public record UpdateVolunteerRequisitesRequest(Guid VolunteerId, UpdateVolunteerRequisitesDto Dto);