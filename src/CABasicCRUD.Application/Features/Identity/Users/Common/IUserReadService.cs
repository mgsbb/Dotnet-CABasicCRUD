using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Common;

public interface IUserReadService
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

    Task<User?> GetByIdAsync(UserId userId);
}
