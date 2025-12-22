using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Domain.Users;
using AuthErrors = CABasicCRUD.Application.Auth.Errors.AuthErrors;

namespace CABasicCRUD.Application.Auth.Commands.LoginUser;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginUserResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return Result<LoginUserResult>.Failure(AuthErrors.InvalidCredentials);
        }

        bool isPasswordCorrect = user.VerifyPassword(request.Password, _passwordHasher);

        if (!isPasswordCorrect)
        {
            return Result<LoginUserResult>.Failure(AuthErrors.InvalidCredentials);
        }

        string token = _jwtProvider.GenerateToken(user);

        LoginUserResult loginUserResult = new(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Token: token
        );

        // return Result<LoginUserResult>.Success(loginUserResult);
        return loginUserResult;
    }
}
