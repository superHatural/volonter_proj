namespace VolunteerProg.Application.Volunteer.Dtos;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber);