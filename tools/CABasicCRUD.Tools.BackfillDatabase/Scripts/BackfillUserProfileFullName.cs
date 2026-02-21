using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Tools.BackfillDatabase.Scripts;

public sealed class BackFillUserProfileFullName(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task ExecuteAsync(bool isDryRun)
    {
        Console.WriteLine(
            isDryRun
                ? $"Running {ScriptNames.BackFillUserProfileFullName} --dry-run"
                : $"Running {ScriptNames.BackFillUserProfileFullName}"
        );

        // update UserProfile FullName when it does not exist for the particular UserId

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var updated = await _dbContext.Database.ExecuteSqlRawAsync(
            """
            UPDATE "UserProfiles" up
            SET
                "FullName" = u."Name"
            FROM "Users" u
            WHERE u."Id" = up."Id"
              AND up."FullName" IS NULL
            """
        );

        Console.WriteLine($"Affected rows: {updated}");

        if (isDryRun)
        {
            await transaction.RollbackAsync();

            Console.WriteLine("Dry run complete. No changes committed.");

            return;
        }

        await transaction.CommitAsync();

        Console.WriteLine("Backfill complete.");
    }
}
