using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer.PetCreate.Create.Requests;

public record CreatePetCommand(
    Guid VolunteerId,
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