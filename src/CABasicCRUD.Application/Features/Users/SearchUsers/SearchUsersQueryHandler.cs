using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.SearchUsers;

internal sealed class SearchUsersQueryHandler(IUserRepository userRepository)
    : IQueryHander<SearchUsersQuery, IReadOnlyList<UserResult>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<IReadOnlyList<UserResult>>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<User> users = await _userRepository.SearchUsersAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            request.OrderBy,
            request.SortDirection,
            cancellationToken
        );

        IReadOnlyList<UserResult> userResults = users.ToListUserResult();

        return Result<IReadOnlyList<UserResult>>.Success(userResults);
    }
}
