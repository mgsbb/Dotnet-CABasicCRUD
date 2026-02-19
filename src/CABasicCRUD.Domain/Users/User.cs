using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Services;

namespace CABasicCRUD.Domain.Users;

public class User : AggregateRoot<UserId>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserProfile? UserProfile { get; private set; }

    private User(UserId id, string name, string email, string passwordHash)
        : base(id)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static Result<User> Create(
        string name,
        string email,
        string password,
        IPasswordHasher passwordHasher
    )
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result<User>.Failure(UserErrors.NameEmpty);
        }
        if (string.IsNullOrEmpty(email))
        {
            return Result<User>.Failure(UserErrors.EmailEmpty);
        }
        if (string.IsNullOrEmpty(password))
        {
            return Result<User>.Failure(UserErrors.PasswordEmpty);
        }

        string passwordHash = HashPassword(password, passwordHasher);

        User user = new(id: UserId.New(), name: name, email: email, passwordHash: passwordHash);

        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Name, user.Email));

        return user;
    }

    private static string HashPassword(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.HashPassword(password);
    }

    public bool VerifyPassword(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.VerifyPassword(password, PasswordHash);
    }

    public Result<User> UpdateDetails(string? name, string? email)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result<User>.Failure(UserErrors.NameEmpty);
        }
        if (string.IsNullOrEmpty(email))
        {
            return Result<User>.Failure(UserErrors.EmailEmpty);
        }
        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}
