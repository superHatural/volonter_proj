using Serilog;
using Serilog.Events;

namespace VolunteerProg.API;

public static class LoggingConfiguration
{
    public static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? 
                         throw new ArgumentNullException("Seq"))
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();
    }
}