using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Tools.BackfillDatabase.Scripts;

public sealed class BackfillUserUsername(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task ExecuteAsync(bool isDryRun)
    {
        Console.WriteLine(
            isDryRun
                ? $"Running {ScriptNames.BackfillUserUsername} --dry-run"
                : $"Running {ScriptNames.BackfillUserUsername}"
        );

        // update User Username when it does not exist for the particular UserId

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var sqlStatement = """
                UPDATE "Users"
                SET "Username" =
                    lower(
                        regexp_replace(
                            coalesce("Name", 'user'),
                            '[^a-zA-Z0-9]',
                            '',
                            'g'
                        )
                    )
                    || '_' ||
                    substring(
                        replace("Id"::text, '-', ''),
                        1,
                        12
                    )
                WHERE "Username" IS NULL
                RETURNING "Id", "Name", "Username"
            """;

        var results = await _dbContext
            .Database.SqlQueryRaw<UsernameResult>(sqlStatement)
            .ToListAsync();

        Console.WriteLine($"Affected rows: {results.Count}");

        foreach (var row in results.Take(100))
        {
            Console.WriteLine($"{row.Id}, {row.Name} -> {row.Username}");
        }

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

public sealed class UsernameResult
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Username { get; init; } = default!;
}
