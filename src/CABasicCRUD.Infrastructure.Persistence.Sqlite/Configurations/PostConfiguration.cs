using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;
using CABasicCRUD.Domain.Posts.Posts;
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

        builder.Property(p => p.CreatedAt).IsRequired();

        builder.Property(p => p.UpdatedAt).IsRequired(false);

        ConfigureMedia(builder);
    }

    private static void ConfigureMedia(EntityTypeBuilder<Post> builder)
    {
        builder.Metadata.FindNavigation(nameof(Post.PostMediaItems))!.SetField("_postMediaItems");

        builder.OwnsMany<PostMedia>(
            post => post.PostMediaItems,
            pmBuilder =>
            {
                pmBuilder.ToTable("PostsMedia");

                pmBuilder.WithOwner();

                pmBuilder.HasKey(pm => new { pm.PostId, pm.MediaId });

                pmBuilder
                    .Property(pm => pm.PostId)
                    .HasConversion(postId => postId.Value, value => (PostId)value);

                pmBuilder
                    .Property(pm => pm.MediaId)
                    .HasConversion(mediaId => mediaId.Value, value => (MediaId)value);
            }
        );
    }
}
