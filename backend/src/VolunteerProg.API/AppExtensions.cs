using Microsoft.EntityFrameworkCore;
using VolunteerProg.Infrastructure;

namespace VolunteerProg.API;

public static class AppExtensions
{
    public static async Task AddMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}