using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Domain.Users;
using AuthErrors = CABasicCRUD.Application.Auth.Errors.AuthErrors;

namespace CABasicCRUD.Application.Auth.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is not null)
        {
            return Result<AuthResult>.Failure(AuthErrors.AlreadyExists);
        }

        Result<User> result = User.Create(
            name: request.Name,
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
            token
        );

        return Result<AuthResult>.Success(authResult);
    }
}
