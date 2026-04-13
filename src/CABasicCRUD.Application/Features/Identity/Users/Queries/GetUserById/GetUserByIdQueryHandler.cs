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
        UserResult? user = await _userReadService.GetByIdWithMediaAsync(request.UserId);

        if (user is null)
        {
            return Result<UserResult>.Failure(UserErrors.NotFound);
        }

        return user;
    }
}
