using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.CreateVolunteer.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> RequisitesRecords,
    IEnumerable<SocialMediaDto> SocialMediaRecords);