using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using VolunteerProg.Application.Voluunter;
using VolunteerProg.Domain.Shared;
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
        var voluunter = await _dbContext.Voluunters
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);
        if (voluunter == null)
            return Errors.General.NotFound(volunteerId);
        return voluunter;
    }
}