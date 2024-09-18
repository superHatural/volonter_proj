using CSharpFunctionalExtensions;
using VolunteerProg.Domain.PetManagement.ValueObjects;
using VolunteerProg.Domain.PetManagement.ValueObjects.Ids;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Volunteer;

public interface IVolunteersRepository
{
    Task<Guid> Add(Domain.PetManagement.AggregateRoot.Volunteer volunteer, CancellationToken cancellationToken);

    Task<Result<Domain.PetManagement.AggregateRoot.Volunteer, Error>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken);

    Task<Result<Domain.PetManagement.AggregateRoot.Volunteer, Error>> GetByPhoneNumber(Phone phoneNumber, CancellationToken cancellationToken);
    Task<Result<Domain.PetManagement.AggregateRoot.Volunteer, Error>> GetByEmail(Email emailResultValue, CancellationToken cancellationToken);
}