using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateRequisites.Requests;

public record UpdateVolunteerRequisitesRequest(Guid VolunteerId, UpdateVolunteerRequisitesDto Dto);