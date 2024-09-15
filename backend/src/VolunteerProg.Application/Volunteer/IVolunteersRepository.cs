using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Ids;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.ValueObjects;
using VolunteerProg.Domain.Volunteers;



public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken);

    Task<Result<Volunteer, Error>> GetByPhoneNumber(Phone phoneNumber, CancellationToken cancellationToken);
    Task<Result<Volunteer, Error>> GetByEmail(Email emailResultValue, CancellationToken cancellationToken);
}