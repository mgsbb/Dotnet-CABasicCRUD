using CABasicCRUD.Application.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.ReadServices;

// ========================================================================================================================
// ========================================================================================================================

public sealed class UserReadService(ApplicationDbContext dbContext) : IUserReadService
{
    private readonly DbSet<User> _dbSet = dbContext.Set<User>();

    // ========================================================================================================================

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    // ========================================================================================================================

    public async Task<IReadOnlyList<User>> SearchUsersAsync(
        string? searchTerm,
        int page,
        int pageSize,
        UserOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    )
    {
        var query = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{searchTerm}%")
                || EF.Functions.ILike(u.Email, $"%{searchTerm}%")
            );
        }

        query = ApplyOrdering(query, orderBy, sortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<User> users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => p)
            .ToListAsync(cancellationToken);

        return users;
    }

    // ========================================================================================================================

    private static IQueryable<User> ApplyOrdering(
        IQueryable<User> query,
        UserOrderBy orderBy,
        SortDirection sortDirection
    )
    {
        return (orderBy, sortDirection) switch
        {
            (UserOrderBy.Name, SortDirection.Asc) => query.OrderBy(u => u.Name),

            (UserOrderBy.Email, SortDirection.Desc) => query.OrderByDescending(u => u.Email),

            (UserOrderBy.CreatedAt, SortDirection.Asc) => query.OrderBy(p => p.CreatedAt),

            _ => query.OrderByDescending(p => p.CreatedAt),
        };
    }

    // ========================================================================================================================

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }

    // ========================================================================================================================

    public async Task<User?> GetByIdAsync(UserId userId)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    // ========================================================================================================================

    public async Task<UserResult?> GetByIdWithMediaAsync(UserId userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserResult(
                u.Id,
                u.UserProfile.FullName,
                u.Email,
                u.Username,
                u.CreatedAt,
                u.UpdatedAt,
                u.UserProfile.Bio,
                dbContext
                    .MediaItems.Where(m => m.Id == u.UserProfile.ProfileImageId)
                    .Select(m => m.Url)
                    .FirstOrDefault(),
                dbContext
                    .MediaItems.Where(m => m.Id == u.UserProfile.CoverImageId)
                    .Select(m => m.Url)
                    .FirstOrDefault()
            ))
            .FirstOrDefaultAsync();
    }

    // ========================================================================================================================

    public async Task<IReadOnlyList<UserResult>> SearchUsersWithMediaAsync(
        string? searchTerm,
        int page,
        int pageSize,
        UserOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    )
    {
        var query = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                EF.Functions.Like(u.Name, $"%{searchTerm}%")
                || EF.Functions.Like(u.Email, $"%{searchTerm}%")
            );
        }

        query = ApplyOrdering(query, orderBy, sortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<UserResult> users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserResult(
                u.Id,
                u.UserProfile.FullName,
                u.Email,
                u.Username,
                u.CreatedAt,
                u.UpdatedAt,
                u.UserProfile.Bio,
                dbContext
                    .MediaItems.Where(m => m.Id == u.UserProfile.ProfileImageId)
                    .Select(m => m.Url)
                    .FirstOrDefault(),
                dbContext
                    .MediaItems.Where(m => m.Id == u.UserProfile.CoverImageId)
                    .Select(m => m.Url)
                    .FirstOrDefault()
            ))
            .ToListAsync(cancellationToken);

        return users;
    }
}

// ========================================================================================================================
// ========================================================================================================================
