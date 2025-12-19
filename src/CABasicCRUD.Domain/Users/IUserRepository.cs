using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByEmailAsync(string email);
}
