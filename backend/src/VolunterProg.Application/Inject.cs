using Microsoft.Extensions.DependencyInjection;
using VolunterProg.Application.Voluunter;
using VolunterProg.Infrastructure.Repositories;

namespace VolunterProg.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVoluunterHandler>();
        return services;
    }
}