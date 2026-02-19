using CABasicCRUD.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

public class UserRepository(ApplicationDbContext dbContext)
    : RepositoryBase<User, UserId>(dbContext),
        IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<IReadOnlyList<User>> SearchUsersAsync(
        string searchTerm,
        int page,
        int pageSize,
        UserOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<UserProfile?> AddUserProfileAsync(UserProfile userProfile)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserProfileAsync(UserProfile userProfile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserProfileAsync(UserProfile userProfile)
    {
        throw new NotImplementedException();
    }
}
