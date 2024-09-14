namespace VolunteerProg.Application.Voluunter.CreateVoluunter;

public record CreateVolunteerRequest(
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
