using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql.Outbox;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        services.AddScoped<OutboxSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) =>
            {
                var interceptor = sp.GetRequiredService<OutboxSaveChangesInterceptor>();

                options.UseNpgsql(connectionString).AddInterceptors(interceptor);
            }
        );

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<OutboxProcessor>();

        services.Configure<DatabaseSeedOptions>(configuration.GetSection("Database"));
        services.AddScoped<ApplicationCommandSeeder>();
        services.AddScoped<RawSqlSeeder>();

        return services;
    }
}
