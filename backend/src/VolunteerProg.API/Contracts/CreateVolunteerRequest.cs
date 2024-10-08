using VolunteerProg.Application.Volunteer.Create;
using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.API.Contracts;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> RequisitesRecords,
    IEnumerable<SocialMediaDto> SocialMediaRecords)
{
    public CreateVolunteerСommand ToCommand()
        => new(FullName,
            Email,
            Description,
            Experience,
            PhoneNumber,
            RequisitesRecords,
            SocialMediaRecords);
}