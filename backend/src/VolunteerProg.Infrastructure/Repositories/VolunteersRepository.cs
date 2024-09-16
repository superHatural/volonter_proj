using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using VolunteerProg.Application.Volunteer;
using VolunteerProg.Domain.Ids;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.ValueObjects;
using VolunteerProg.Domain.Volunteers;

namespace VolunteerProg.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VolunteersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken)
    {
        await _dbContext.Voluunters.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext.Voluunters
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.NotFound(volunteerId);
        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(Phone phoneNumber, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext.Voluunters
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(phoneNumber);
        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByEmail(Email emailAddress, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext.Voluunters
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Email == emailAddress, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(emailAddress);
        return volunteer;
    }
}