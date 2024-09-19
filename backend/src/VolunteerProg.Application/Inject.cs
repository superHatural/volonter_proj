using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VolunteerProg.Application.Volunteer.CreateVolunteer;
using VolunteerProg.Application.Volunteer.CreateVolunteer.Handlers;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Handler;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateRequisites.Handlers;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Handlers;

namespace VolunteerProg.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerRequisitesHandler>();
        services.AddScoped<UpdateVolunteerMainInfoHandler>();
        services.AddScoped<UpdateVolunteerSocialMediaHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}