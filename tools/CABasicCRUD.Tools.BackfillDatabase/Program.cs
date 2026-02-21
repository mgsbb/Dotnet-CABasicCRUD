using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using CABasicCRUD.Tools.BackfillDatabase;
using CABasicCRUD.Tools.BackfillDatabase.Scripts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

if (args.Length == 0)
{
    Console.WriteLine("Specify available scripts:");
    Console.WriteLine(ScriptNames.CreateMissingUserProfiles + " [--dry-run]");
    Console.WriteLine(ScriptNames.BackFillUserProfileFullName + " [--dry-run]");
    Console.WriteLine(ScriptNames.VerifyUserProfileFullName);
    return;
}

var scriptName = args[0].ToLower();

var isDryRun = args.Any(a => a.Equals("--dry-run", StringComparison.OrdinalIgnoreCase));

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    (context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<CreateMissingUserProfiles>();
        services.AddScoped<BackFillUserProfileFullName>();
        services.AddScoped<VerifyUserProfileFullName>();
    }
);

var host = builder.Build();

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

switch (scriptName)
{
    case ScriptNames.CreateMissingUserProfiles:
        await services.GetRequiredService<CreateMissingUserProfiles>().ExecuteAsync(isDryRun);
        break;
    case ScriptNames.BackFillUserProfileFullName:
        await services.GetRequiredService<BackFillUserProfileFullName>().ExecuteAsync(isDryRun);
        break;
    case ScriptNames.VerifyUserProfileFullName:
        await services.GetRequiredService<VerifyUserProfileFullName>().ExecuteAsync();
        break;
    default:
        Console.WriteLine("Unknown script");
        break;
}
