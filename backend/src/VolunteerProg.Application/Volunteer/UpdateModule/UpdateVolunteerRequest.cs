using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.UpdateModule;

public record UpdateVolunteerRequest(
    Guid Id, 
    VolunteerDto VolunteerDto);