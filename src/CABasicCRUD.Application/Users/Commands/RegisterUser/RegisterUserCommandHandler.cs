using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Users.DTOs;
using CABasicCRUD.Application.Users.Mapping;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Domain.Users;
using UserErrors = CABasicCRUD.Application.Users.Errors.UserErrors;

namespace CABasicCRUD.Application.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByEmailAsync(request.RegisterUserRequest.Email);

        if (user is not null)
        {
            return Result<UserResponse>.Failure(UserErrors.AlreadyExists);
        }

        Result<User> userResult = User.Create(
            name: request.RegisterUserRequest.Name,
            email: request.RegisterUserRequest.Email,
            password: request.RegisterUserRequest.Password,
            passwordHasher: _passwordHasher
        );

        if (userResult.IsFailure || userResult.Value is null)
        {
            return Result<UserResponse>.Failure(userResult.Error);
        }

        User registeredUser = userResult.Value;

        await _userRepository.AddAsync(registeredUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        UserResponse userResponse = registeredUser.ToUserResponse();

        return Result<UserResponse>.Success(userResponse);
    }
}
