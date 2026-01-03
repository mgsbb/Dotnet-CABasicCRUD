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
}
