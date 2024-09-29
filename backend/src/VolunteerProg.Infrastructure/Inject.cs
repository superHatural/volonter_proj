using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.Providers;
using VolunteerProg.Application.Volunteer;
using VolunteerProg.Infrastructure.Options;
using VolunteerProg.Infrastructure.Providers;
using VolunteerProg.Infrastructure.Repositories;

namespace VolunteerProg.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        
        services.AddMinio(configuration);

        return services;
    }

    private static IServiceCollection AddMinio(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SECTIONNAME));
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.SECTIONNAME).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
    
}