using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Avoid this, otherwise factory is coupled to a particular host, use environment variables instead
        // string hostAssemblyName = "CABasicCRUD.Host";

        // Assembly hostAssembly =
        //     Assembly.Load(hostAssemblyName)
        //     ?? throw new InvalidOperationException(
        //         $"Could not load startup host assembly: {hostAssemblyName}"
        //     );

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            // .AddJsonFile("appsettings.json", optional: true)
            // .AddJsonFile("appsettings.Development.json", optional: true)
            // .AddUserSecrets(hostAssembly)
            .AddEnvironmentVariables()
            .Build();

        string connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        DbContextOptions<ApplicationDbContext> options =
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connectionString).Options;

        return new ApplicationDbContext(options);
    }
}
