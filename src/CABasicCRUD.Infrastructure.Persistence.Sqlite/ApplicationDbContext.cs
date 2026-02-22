using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;
using CABasicCRUD.Domain.Posts.Posts;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Conversation> Conversations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
