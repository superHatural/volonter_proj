using VolunteerProg.Application.Volunteer.Create.Requests;
using VolunteerProg.Application.Volunteer.Dtos;

namespace VolunteerProg.API.Contracts;

public record CreatePetRequest(
    string Name,
    string Description,
    Guid Species,
    Guid Breed,
    string Color,
    string HealthInfo,
    AddressDto Address,
    int Weight,
    int Height,
    string Phone,
    bool IsCastrated,
    string BirthDate,
    bool IsVaccinated,
    string Status,
    IEnumerable<RequisiteDto> RequisitesRecords
);