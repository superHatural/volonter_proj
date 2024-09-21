using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VolunteerProg.Application.Volunteer.Create.Handlers;
using VolunteerProg.Application.Volunteer.Delete.Handlers;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileHandler;
using VolunteerProg.Application.Volunteer.PetCreate.DeleteFile.DeleteFileHandler;
using VolunteerProg.Application.Volunteer.PetCreate.GetFiles.GetFilesHandler;
using VolunteerProg.Application.Volunteer.Update.UpdateMainInfo.Handler;
using VolunteerProg.Application.Volunteer.Update.UpdateRequisites.Handlers;
using VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia.Handlers;

namespace VolunteerProg.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerRequisitesHandler>();
        services.AddScoped<UpdateVolunteerMainInfoHandler>();
        services.AddScoped<UpdateVolunteerSocialMediaHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<AddFileHandler>();
        services.AddScoped<GetFilesHandler>();
        services.AddScoped<DeleteFileHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}