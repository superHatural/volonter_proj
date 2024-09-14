using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;
using VolunterProg.Domain.Voluunters;

namespace VolunterProg.Infrastructure.Repositories;

public interface IVoluuntersRepository
{
    Task<Guid> Add(Voluunter voluunter, CancellationToken cancellationToken);
    Task<Result<Voluunter, Error>> GetById(VoluunterId voluunterId, CancellationToken cancellationToken);
}