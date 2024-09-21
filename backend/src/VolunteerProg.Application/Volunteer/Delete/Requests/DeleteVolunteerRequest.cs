using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.Delete.Requests;

public record DeleteVolunteerRequest(
    Guid VolunteerId);