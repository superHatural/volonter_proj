using Microsoft.Extensions.DependencyInjection;
using VolunterProg.Infrastructure.Repositories;

namespace VolunterProg.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IVoluuntersRepository, VoluuntersRepository>();
        return services;
    }
}