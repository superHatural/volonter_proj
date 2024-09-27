using VolunteerProg.Application.Volunteer.Create.Requests;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.PetCreate.Create.Requests;

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
)
{
    public CreatePetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            Name,
            Description,
            Species,
            Breed,
            Color,
            HealthInfo,
            Address,
            Weight,
            Height,
            Phone,
            IsCastrated,
            BirthDate,
            IsVaccinated,
            Status,
            RequisitesRecords);
}