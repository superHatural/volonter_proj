using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.Update.UpdateRequisites;

public record UpdateVolunteerRequisitesCommand(Guid VolunteerId, UpdateVolunteerRequisitesDto Dto);