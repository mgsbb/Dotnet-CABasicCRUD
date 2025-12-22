using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Users.DTOs;
using CABasicCRUD.Application.Users.Mapping;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Domain.Users;
using UserErrors = CABasicCRUD.Application.Users.Errors.UserErrors;

namespace CABasicCRUD.Application.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserResult>
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

    public async Task<Result<UserResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is not null)
        {
            return Result<UserResult>.Failure(UserErrors.AlreadyExists);
        }

        Result<User> result = User.Create(
            name: request.Name,
            email: request.Email,
            password: request.Password,
            passwordHasher: _passwordHasher
        );

        if (result.IsFailure || result.Value is null)
        {
            return Result<UserResult>.Failure(result.Error);
        }

        User registeredUser = result.Value;

        await _userRepository.AddAsync(registeredUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        UserResult userResult = registeredUser.ToUserResult();

        return Result<UserResult>.Success(userResult);
    }
}
