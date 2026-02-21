using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public class UserRepository(ApplicationDbContext dbContext)
    : RepositoryBase<User, UserId>(dbContext),
        IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IReadOnlyList<User>> SearchUsersAsync(
        string searchTerm,
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

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }
}
