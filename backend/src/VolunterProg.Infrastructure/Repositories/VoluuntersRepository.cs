using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;

namespace VolunterProg.Infrastructure.Repositories;

public class VoluuntersRepository : IVoluuntersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VoluuntersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Add(Voluunter voluunter, CancellationToken cancellationToken)
    {
        await _dbContext.Voluunters.AddAsync(voluunter, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success((Guid)voluunter.Id);
    }

    public async Task<Result<Voluunter, Error>> GetById(VoluunterId voluunterId, CancellationToken cancellationToken)
    {
        var voluunter = await _dbContext.Voluunters
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == voluunterId, cancellationToken);
        if (voluunter == null)
            return Errors.General.NotFound(voluunterId);
        return voluunter;
    }
}