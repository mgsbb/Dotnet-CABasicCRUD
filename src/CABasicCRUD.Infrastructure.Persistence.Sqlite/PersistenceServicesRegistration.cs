using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Infrastructure.Persistence.Sqlite.Outbox;
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
        string connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        services.AddScoped<OutboxSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) =>
            {
                var interceptor = sp.GetRequiredService<OutboxSaveChangesInterceptor>();

                options.UseSqlite(connectionString).AddInterceptors(interceptor);
            }
        );

        services.AddMemoryCache();

        services.AddScoped(typeof(RepositoryBase<,>));

        services.AddScoped<IPostRepository, CachedPostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
