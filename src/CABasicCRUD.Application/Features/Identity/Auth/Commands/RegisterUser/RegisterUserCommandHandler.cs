using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Auth.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserReadService _userReadService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IUserReadService userReadService
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _userReadService = userReadService;
    }

    public async Task<Result<AuthResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userReadService.GetByEmailAsync(request.Email);

        if (user is not null)
        {
            return Result<AuthResult>.Failure(AuthErrors.AlreadyExistsEmail);
        }

        User? userByUsername = await _userReadService.GetByUsernameAsync(request.Username);

        if (userByUsername is not null)
        {
            return Result<AuthResult>.Failure(AuthErrors.AlreadyExistsUsername);
        }

        Result<User> result = User.Create(
            name: request.Name,
            username: request.Username,
            email: request.Email,
            password: request.Password,
            passwordHasher: _passwordHasher
        );

        if (result.IsFailure || result.Value is null)
        {
            return Result<AuthResult>.Failure(result.Error);
        }

        User registeredUser = result.Value;

        await _userRepository.AddAsync(registeredUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        string token = _jwtProvider.GenerateToken(registeredUser);

        AuthResult authResult = new(
            registeredUser.Id,
            registeredUser.Name,
            registeredUser.Email,
            token,
            registeredUser.Username,
            registeredUser.CreatedAt,
            registeredUser.UpdatedAt
        );

        return Result<AuthResult>.Success(authResult);
    }
}
