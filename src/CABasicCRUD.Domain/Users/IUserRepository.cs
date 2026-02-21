using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByUsernameAsync(string username);

    Task<IReadOnlyList<User>> SearchUsersAsync(
        string searchTerm,
        int page,
        int pageSize,
        UserOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    );
}
