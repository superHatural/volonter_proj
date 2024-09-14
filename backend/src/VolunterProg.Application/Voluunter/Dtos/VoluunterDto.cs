namespace VolunterProg.Application.Voluunter;

public record VoluunterDto(
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