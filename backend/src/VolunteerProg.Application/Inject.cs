using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VolunteerProg.Application.Volunteer.CreateVolunteer;

namespace VolunteerProg.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}