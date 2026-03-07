using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Posts.Comments;
using CABasicCRUD.Domain.Posts.Posts;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

// ========================================================================================================================
// ========================================================================================================================

public class CommentReadService(ApplicationDbContext dbContext) : ICommentReadService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly DbSet<Comment> _dbSet = dbContext.Set<Comment>();

    // ========================================================================================================================

    public async Task<IReadOnlyList<Comment>> GetAllCommentsOfPost(PostId postId)
    {
        return await _dbSet.AsNoTracking().Where(comment => comment.PostId == postId).ToListAsync();
    }

    // ========================================================================================================================

    public async Task<Comment?> GetByIdAsync(CommentId commentId)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(comment => comment.Id == commentId);
    }

    // ========================================================================================================================

    public async Task<IReadOnlyList<CommentWithAuthorResult>> GetCommentsWithAuthorOfPostAsync(
        PostId postId,
        CancellationToken cancellationToken
    )
    {
        return await (
            from c in _dbContext.Comments
            join u in _dbContext.Users on c.UserId equals u.Id
            where c.PostId == postId
            select new CommentWithAuthorResult(
                c.Id,
                c.Body,
                c.PostId,
                c.UserId,
                u.Name,
                c.CreatedAt,
                c.UpdatedAt
            )
        )
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

// ========================================================================================================================
// ========================================================================================================================
