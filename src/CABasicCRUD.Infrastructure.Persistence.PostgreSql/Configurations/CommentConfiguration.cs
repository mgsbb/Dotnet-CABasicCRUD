using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(comment => comment.Id);
        builder
            .Property(comment => comment.Id)
            .HasConversion(commentId => commentId.Value, value => (CommentId)value);

        builder.Property(comment => comment.Body).IsRequired();

        builder.HasOne<Post>().WithMany().HasForeignKey(comment => comment.PostId);
        builder
            .Property(comment => comment.PostId)
            .HasConversion(postId => postId.Value, value => (PostId)value);

        builder.HasOne<User>().WithMany().HasForeignKey(comment => comment.UserId);
        builder
            .Property(comment => comment.UserId)
            .HasConversion(userId => userId.Value, value => (UserId)value);
    }
}
