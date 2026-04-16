using CABasicCRUD.Application.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

// ========================================================================================================================
// ========================================================================================================================

public class PostReadService(ApplicationDbContext dbContext) : IPostReadService
{
    private readonly DbSet<Post> _dbSet = dbContext.Set<Post>();
    protected readonly ApplicationDbContext _dbContext = dbContext;

    // ========================================================================================================================

    public async Task<IReadOnlyList<PostWithAuthorResult>> SearchPostsAsync(
        string? searchTerm,
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
                (post, user) => new { post, user }
            )
            .SelectMany(
                pu =>
                    _dbContext
                        .MediaItems.Where(media => media.Id == pu.user.UserProfile.ProfileImageId)
                        .DefaultIfEmpty(),
                (pu, profileImage) =>
                    new
                    {
                        pu.user,
                        pu.post,
                        profileImage,
                    }
            )
            .Select(x => new PostWithAuthorResult(
                x.post.Id,
                x.post.Title,
                x.post.Content,
                x.post.UserId,
                x.user.UserProfile.FullName,
                x.profileImage != null ? x.profileImage.Url : null,
                x.post.CreatedAt,
                x.post.UpdatedAt,
                x.post.PostMediaItems.Join(
                        _dbContext.MediaItems,
                        pm => pm.MediaId,
                        m => m.Id,
                        (pm, m) => m.Url
                    )
                    .ToList()
            ))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return posts;
    }

    // ========================================================================================================================

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

    // ========================================================================================================================

    public async Task<PostWithAuthorResult?> GetPostByIdWithAuthor(
        PostId postId,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext
            .Posts.AsNoTracking()
            .Where(post => post.Id == postId)
            .Join(
                _dbContext.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) => new { post, user }
            )
            .SelectMany(
                pu =>
                    _dbContext
                        .MediaItems.Where(media => media.Id == pu.user.UserProfile.ProfileImageId)
                        .DefaultIfEmpty(),
                (pu, profileImage) =>
                    new
                    {
                        pu.user,
                        pu.post,
                        profileImage,
                    }
            )
            .Select(x => new PostWithAuthorResult(
                x.post.Id,
                x.post.Title,
                x.post.Content,
                x.post.UserId,
                x.user.UserProfile.FullName,
                x.profileImage != null ? x.profileImage.Url : null,
                x.post.CreatedAt,
                x.post.UpdatedAt,
                x.post.PostMediaItems.Join(
                        _dbContext.MediaItems,
                        pm => pm.MediaId,
                        m => m.Id,
                        (pm, m) => m.Url
                    )
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    // ========================================================================================================================

    public async Task<IReadOnlyList<Post>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    // ========================================================================================================================

    public async Task<Post?> GetByIdAsync(PostId id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(post => post.Id == id);
    }

    // ========================================================================================================================

    public async Task<PostResult?> GetByIdWithMediaAsync(PostId id)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(post => post.Id == id)
            .Select(post => new PostResult(
                post.Id,
                post.Title,
                post.Content,
                post.UserId,
                post.CreatedAt,
                post.UpdatedAt,
                post.PostMediaItems.Join(
                        _dbContext.MediaItems,
                        pm => pm.MediaId,
                        m => m.Id,
                        (pm, m) => m.Url
                    )
                    .ToList()
            ))
            .FirstOrDefaultAsync();
    }

    // ========================================================================================================================

    public async Task<IReadOnlyList<PostResult>> GetAllWithMediaAsync()
    {
        return await _dbContext
            .Posts.AsNoTracking()
            .Join(
                _dbContext.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) =>
                    new
                    {
                        post,
                        user,
                        mediaUrls = post.PostMediaItems.Join(
                            _dbContext.MediaItems,
                            pm => pm.MediaId,
                            media => media.Id,
                            (pm, m) => m.Url
                        ),
                    }
            )
            .Select(x => new PostResult(
                x.post.Id,
                x.post.Title,
                x.post.Content,
                x.post.UserId,
                x.post.CreatedAt,
                x.post.UpdatedAt,
                x.mediaUrls.ToList()
            ))
            .ToListAsync();
    }
}

// ========================================================================================================================
// ========================================================================================================================
