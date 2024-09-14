namespace VolunterProg.Application.Voluunter;

public record CreateVoluunterRequest(
    string FirstName, 
    string LastName, 
    string Email, 
    string Description,
    int Experience, 
    string PhoneNumber,
    string? RequisiteTitle,
    string? RequisiteDescription,
    string? SocMedTitle,
    string? SocMedUrl);
