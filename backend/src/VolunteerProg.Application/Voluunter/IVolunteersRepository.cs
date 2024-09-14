using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Volunteers;

namespace VolunteerProg.Application.Voluunter;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);
    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);
}