using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using UserErrors = CABasicCRUD.Application.Features.Identity.Users.Common.UserErrors;

namespace CABasicCRUD.Application.Features.Identity.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserReadService userReadService)
    : IQueryHander<GetUserByIdQuery, UserResult>
{
    private readonly IUserReadService _userReadService = userReadService;

    public async Task<Result<UserResult>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userReadService.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<UserResult>.Failure(UserErrors.NotFound);
        }

        UserResult userResult = user.ToUserResult();

        return userResult;
    }
}
