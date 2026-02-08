namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Seeding;

public sealed class DatabaseSeedOptions
{
    public bool IsSeedDatabase { get; init; }
    public string SeedUserPassword { get; init; } = null!;
}
