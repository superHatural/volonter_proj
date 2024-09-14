using Microsoft.Extensions.DependencyInjection;
using VolunteerProg.Application.Voluunter.CreateVoluunter;

namespace VolunteerProg.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        return services;
    }
}