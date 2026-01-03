using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public class CommentRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Comment, CommentId>(dbContext),
        ICommentRepository
{
    public async Task<IReadOnlyList<Comment>> GetAllCommentsOfPost(PostId postId)
    {
        IReadOnlyList<Comment> comments = await _dbSet
            .Where(comment => comment.PostId == postId)
            .ToListAsync();

        return comments;
    }
}
