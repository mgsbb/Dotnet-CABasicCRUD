using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHander<GetUserByIdQuery, UserResult>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResult>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<UserResult>.Failure(UserErrors.NotFound);
        }

        UserResult userResult = user.ToUserResult();

        return userResult;
    }
}
