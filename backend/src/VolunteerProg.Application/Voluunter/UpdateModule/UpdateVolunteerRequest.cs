using VolunteerProg.Application.Voluunter.Dtos;

namespace VolunteerProg.Application.Voluunter.UpdateModule;

public record UpdateVolunteerRequest(
    Guid Id, 
    VolunteerDto VolunteerDto);