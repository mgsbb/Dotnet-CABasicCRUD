using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.DeleteUser;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result.Failure(AuthErrors.Unauthenticated);
        }

        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (_currentUser.UserId != request.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        await _userRepository.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
