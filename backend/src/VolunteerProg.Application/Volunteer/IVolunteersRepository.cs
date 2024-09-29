using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Application.Volunteer;

public interface IVolunteersRepository
{
    Task<Guid> Add(Domain.Aggregates.PetManagement.AggregateRoot.Volunteer volunteer, CancellationToken cancellationToken);
    Guid Save(Domain.Aggregates.PetManagement.AggregateRoot.Volunteer volunteer, CancellationToken cancellationToken);
    Task<Result<Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken);

    Task<Result<Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>> GetByPhoneNumber(Phone phoneNumber, CancellationToken cancellationToken);
    Task<Result<Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>> GetByEmail(Email emailResultValue, CancellationToken cancellationToken);
}