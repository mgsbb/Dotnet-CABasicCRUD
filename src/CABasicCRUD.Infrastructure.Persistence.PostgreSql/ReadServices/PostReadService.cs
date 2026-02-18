using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using SortDirection = CABasicCRUD.Application.Features.Posts.SortDirection;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

public class PostReadService(ApplicationDbContext dbContext) : IPostReadService
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<PostWithAuthorResult>> SearchPostsAsync(
        string searchTerm,
        int page,
        int pageSize,
        PostOrderBy orderBy,
        SortDirection sortDirection,
        UserId? userId,
        CancellationToken cancellationToken
    )
    {
        var query = _dbContext.Posts.AsNoTracking();

        if (userId is not null)
        {
            query = query.Where(p => p.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                EF.Functions.ILike(p.Title, $"%{searchTerm}%")
                || EF.Functions.ILike(p.Content, $"%{searchTerm}%")
            );
        }

        query = ApplyOrdering(query, orderBy, sortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<PostWithAuthorResult> posts = await query
            .Join(
                _dbContext.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) =>
                    new PostWithAuthorResult(
                        post.Id,
                        post.Title,
                        post.Content,
                        post.UserId,
                        user.Name,
                        post.CreatedAt,
                        post.UpdatedAt
                    )
            )
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return posts;
    }

    private static IQueryable<Post> ApplyOrdering(
        IQueryable<Post> query,
        PostOrderBy orderBy,
        SortDirection sortDirection
    )
    {
        return (orderBy, sortDirection) switch
        {
            (PostOrderBy.Title, SortDirection.Asc) => query.OrderBy(p => p.Title),

            (PostOrderBy.Title, SortDirection.Desc) => query.OrderByDescending(p => p.Title),

            (PostOrderBy.CreatedAt, SortDirection.Asc) => query.OrderBy(p => p.CreatedAt),

            _ => query.OrderByDescending(p => p.CreatedAt),
        };
    }
}
