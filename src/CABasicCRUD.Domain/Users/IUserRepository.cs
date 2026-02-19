using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByEmailAsync(string email);

    Task<IReadOnlyList<User>> SearchUsersAsync(
        string searchTerm,
        int page,
        int pageSize,
        UserOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    );

    Task<UserProfile?> AddUserProfileAsync(UserProfile userProfile);

    Task UpdateUserProfileAsync(UserProfile userProfile);

    Task DeleteUserProfileAsync(UserProfile userProfile);
}
