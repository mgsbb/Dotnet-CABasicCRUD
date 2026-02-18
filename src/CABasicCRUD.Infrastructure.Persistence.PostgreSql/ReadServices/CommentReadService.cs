using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

public class CommentReadService(ApplicationDbContext dbContext) : ICommentReadService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

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
