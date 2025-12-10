using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CABasicCRUD.Infrastructure.Persistence;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IPostRepository, PostRepository>();

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        return services;
    }
}
