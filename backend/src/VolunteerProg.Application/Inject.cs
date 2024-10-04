using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VolunteerProg.Application.Volunteer.Create;
using VolunteerProg.Application.Volunteer.Delete;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile;
using VolunteerProg.Application.Volunteer.PetCreate.Create;
using VolunteerProg.Application.Volunteer.PetCreate.DeleteFile;
using VolunteerProg.Application.Volunteer.PetCreate.GetFiles;
using VolunteerProg.Application.Volunteer.Update.UpdateMainInfo;
using VolunteerProg.Application.Volunteer.Update.UpdateRequisites;
using VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia;

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
        services.AddScoped<CreatePetHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}