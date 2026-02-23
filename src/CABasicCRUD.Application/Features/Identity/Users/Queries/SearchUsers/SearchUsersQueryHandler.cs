using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Queries.SearchUsers;

internal sealed class SearchUsersQueryHandler(IUserReadService userReadService)
    : IQueryHander<SearchUsersQuery, IReadOnlyList<UserResult>>
{
    private readonly IUserReadService _userReadService = userReadService;

    public async Task<Result<IReadOnlyList<UserResult>>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<User> users = await _userReadService.SearchUsersAsync(
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
