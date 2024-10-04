using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.Application.Volunteer.Create;

public record CreateVolunteerСommand(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> RequisitesRecords,
    IEnumerable<SocialMediaDto> SocialMediaRecords);