using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Auth.Commands.LoginUser;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthResult>
{
    private readonly IUserReadService _userReadService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserReadService userReadService,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider
    )
    {
        _userReadService = userReadService;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userReadService.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return Result<AuthResult>.Failure(AuthErrors.InvalidCredentials);
        }

        bool isPasswordCorrect = user.VerifyPassword(request.Password, _passwordHasher);

        if (!isPasswordCorrect)
        {
            return Result<AuthResult>.Failure(AuthErrors.InvalidCredentials);
        }

        string token = _jwtProvider.GenerateToken(user);

        AuthResult authResult = new(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Token: token,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt,
            Username: user.Username
        );

        // return Result<LoginUserResult>.Success(loginUserResult);
        return authResult;
    }
}
