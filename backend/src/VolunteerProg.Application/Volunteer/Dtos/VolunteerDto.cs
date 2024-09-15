namespace VolunteerProg.Application.Volunteer.Dtos;

public record VolunteerDto(
    string FirstName, 
    string LastName, 
    string Email, 
    string Description,
    int Experience, 
    string PhoneNumber,
    string? RequisiteTitle,
    string? RequisiteDescription,
    string? SocMedTitle,
    string? SocMedDescription);