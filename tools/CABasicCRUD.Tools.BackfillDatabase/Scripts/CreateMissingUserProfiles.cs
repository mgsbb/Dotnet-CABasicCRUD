using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Tools.BackfillDatabase.Scripts;

public sealed class CreateMissingUserProfiles(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task ExecuteAsync(bool isDryRun)
    {
        Console.WriteLine(
            isDryRun
                ? $"Running {ScriptNames.CreateMissingUserProfiles} --dry-run"
                : $"Running {ScriptNames.CreateMissingUserProfiles}"
        );

        // insert UserProfiles when it does not exist for the particular UserId
        var insertCount = await _dbContext
            .Database.SqlQuery<int>(
                $"""
                    SELECT COUNT(*) AS "Value"
                    FROM "Users" u
                    LEFT JOIN "UserProfiles" up ON up."Id" = u."Id"
                    WHERE up."Id" IS NULL
                      AND u."Name" IS NOT NULL
                """
            )
            .SingleAsync();

        Console.WriteLine($"Would insert: {insertCount}");

        if (isDryRun)
        {
            Console.WriteLine("Dry run complete. No changes executed.");
            return;
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var inserted = await _dbContext.Database.ExecuteSqlRawAsync(
            """
                INSERT INTO "UserProfiles" ("Id", "FullName", "Bio", "ProfileImageUrl", "CreatedAt", "UpdatedAt")
                SELECT u."Id", u."Name", NULL, NULL, NOW(), NULL
                FROM "Users" u
                LEFT JOIN "UserProfiles" up ON up."Id" = u."Id"
                WHERE up."Id" IS NULL
            """
        );

        await transaction.CommitAsync();

        Console.WriteLine($"Inserted: {inserted}");
    }
}
