using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(postId => postId.Value, value => (PostId)value);

        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);

        builder.Property(p => p.Content).IsRequired();

        builder.HasOne<User>().WithMany().HasForeignKey(p => p.UserId);
        builder
            .Property(p => p.UserId)
            .HasConversion(userId => userId.Value, value => (UserId)value);
    }
}
