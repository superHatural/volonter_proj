using Microsoft.Extensions.DependencyInjection;
using VolunteerProg.Application.Voluunter;
using VolunteerProg.Infrastructure.Repositories;

namespace VolunteerProg.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        return services;
    }
}