using System.Diagnostics;
using Bogus;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Seeding;

public sealed class RawSqlSeeder(
    ApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    ILogger<RawSqlSeeder> logger,
    IOptions<DatabaseSeedOptions> options
)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ILogger<RawSqlSeeder> _logger = logger;
    private readonly DatabaseSeedOptions _options = options.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!_options.IsSeedDatabase)
        {
            return;
        }

        _logger.LogInformation("Running database seeder.");

        Guid? defaultSeededUserId = await _dbContext
            .Database.SqlQueryRaw<Guid?>(
                """SELECT "Id" AS "Value" FROM "Users" WHERE "Email" = @Email""",
                new NpgsqlParameter("@Email", "default_user@email.com")
            )
            .FirstOrDefaultAsync(cancellationToken);

        if (defaultSeededUserId is not null)
        {
            _logger.LogInformation("Database already seeded.");
            return;
        }

        var passwordHash = _passwordHasher.HashPassword(_options.SeedUserPassword);

        Randomizer.Seed = new Random(100);

        var userIds = new List<UserId>();
        var postIds = new List<PostId>();

        var faker = new Faker();

        var stopWatch = Stopwatch.StartNew();

        UserId defaultUserId = UserId.New();

        await _dbContext.Database.ExecuteSqlRawAsync(
            """
                INSERT INTO "Users" ("Id", "Name", "Email", "PasswordHash", "CreatedAt", "UpdatedAt")
                VALUES (@Id, @Name, @Email, @PasswordHash, @CreatedAt, NULL)
            """,
            new NpgsqlParameter("@Id", defaultUserId.Value),
            new NpgsqlParameter("@Name", "John Doe"),
            new NpgsqlParameter("@Email", "default_user@email.com"),
            new NpgsqlParameter("@PasswordHash", passwordHash),
            new NpgsqlParameter("@CreatedAt", DateTime.UtcNow)
        );

        userIds.Add(defaultUserId);

        // 49 users
        for (var i = 0; i < 49; i++)
        {
            var userId = UserId.New();

            await _dbContext.Database.ExecuteSqlRawAsync(
                """
                   INSERT INTO "Users" ("Id", "Name", "Email", "PasswordHash", "CreatedAt", "UpdatedAt")
                    VALUES (@Id, @Name, @Email, @PasswordHash, @CreatedAt, NULL)
                """,
                new NpgsqlParameter("@Id", userId.Value),
                new NpgsqlParameter("@Name", faker.Name.FullName()),
                new NpgsqlParameter("@Email", faker.Internet.Email()),
                new NpgsqlParameter("@PasswordHash", passwordHash),
                new NpgsqlParameter("@CreatedAt", DateTime.UtcNow)
            );

            userIds.Add(userId);
        }

        // each user creates 10 posts, so 500 posts
        foreach (UserId userId in userIds)
        {
            for (var i = 0; i < 10; i++)
            {
                string postContent = string.Join(
                    " ",
                    Enumerable.Range(0, 10).Select(_ => faker.Hacker.Phrase())
                );

                PostId postId = PostId.New();

                await _dbContext.Database.ExecuteSqlRawAsync(
                    """
                        INSERT INTO "Posts" ("Id", "Title", "Content", "UserId", "CreatedAt", "UpdatedAt")
                        VALUES (@Id, @Title, @Content, @UserId, @CreatedAt, NULL)
                    """,
                    new NpgsqlParameter("@Id", postId.Value),
                    new NpgsqlParameter("@Title", faker.Commerce.ProductName()),
                    new NpgsqlParameter("@Content", postContent),
                    new NpgsqlParameter("@UserId", userId.Value),
                    new NpgsqlParameter("@CreatedAt", DateTime.UtcNow)
                );

                postIds.Add(postId);
            }
        }

        // each post has 10 comments, so 5000 comments
        foreach (PostId postId in postIds)
        {
            for (var i = 0; i < 10; i++)
            {
                var userId = faker.PickRandom(userIds);

                CommentId commentId = CommentId.New();

                await _dbContext.Database.ExecuteSqlRawAsync(
                    """
                        INSERT INTO "Comments" ("Id", "Body", "PostId", "UserId", "CreatedAt", "UpdatedAt")
                        VALUES (@Id, @Body, @PostId, @UserId, @CreatedAt, NULL)
                    """,
                    new NpgsqlParameter("@Id", commentId.Value),
                    new NpgsqlParameter("@Body", faker.Commerce.ProductName()),
                    new NpgsqlParameter("@PostId", postId.Value),
                    new NpgsqlParameter("@UserId", userId.Value),
                    new NpgsqlParameter("@CreatedAt", DateTime.UtcNow)
                );
            }
        }

        stopWatch.Stop();

        _logger.LogInformation("Seeding took {elapsedTime}", stopWatch.Elapsed);
    }
}
