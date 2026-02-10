using CABasicCRUD.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public class PostRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Post, PostId>(dbContext),
        IPostRepository
{
    public async Task<IReadOnlyList<Post>> SearchPostsAsync(
        string searchTerm,
        int page,
        int pageSize,
        PostOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    )
    {
        var query = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                EF.Functions.ILike(p.Title, $"%{searchTerm}%")
                || EF.Functions.ILike(p.Content, $"%{searchTerm}%")
            );
        }

        query = ApplyOrdering(query, orderBy, sortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<Post> posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => p)
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
