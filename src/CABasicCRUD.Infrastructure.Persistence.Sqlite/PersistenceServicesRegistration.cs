using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite;

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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        return services;
    }
}
