using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Tools.BackfillDatabase.Scripts;

public sealed class VerifyUserProfileFullName
{
    private readonly ApplicationDbContext _dbContext;

    public VerifyUserProfileFullName(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("Verifying User.Name == UserProfile.FullName");

        var missingProfiles = await _dbContext
            .Database.SqlQuery<int>(
                $"""
                SELECT COUNT(*) AS "Value"
                FROM "Users" u
                LEFT JOIN "UserProfiles" up ON up."Id" = u."Id"
                WHERE up."Id" IS NULL
                """
            )
            .SingleAsync();

        var mismatchedNames = await _dbContext
            .Database.SqlQuery<int>(
                $"""
                SELECT COUNT(*) AS "Value"
                FROM "Users" u
                JOIN "UserProfiles" up ON up."Id" = u."Id"
                WHERE up."FullName" IS DISTINCT FROM u."Name"
                """
            )
            .SingleAsync();

        Console.WriteLine($"Missing profiles: {missingProfiles}");

        Console.WriteLine($"Mismatched names: {mismatchedNames}");

        if (missingProfiles == 0 && mismatchedNames == 0)
        {
            Console.WriteLine("Verification PASSED");
        }
        else
        {
            Console.WriteLine("Verification FAILED");
        }
    }
}
