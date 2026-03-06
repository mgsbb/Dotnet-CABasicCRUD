using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.UserProfiles;

namespace CABasicCRUD.Domain.Identity.Users;

public class User : AggregateRoot<UserId>
{
    // can remove name, drop name from db
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public string Username { get; private set; }

    private User(UserId id, string name, string email, string passwordHash, string username)
        : base(id)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        UserProfile = UserProfile.Create(id, Name, null, null);
        Username = username;
    }

    public static Result<User> Create(
        string name,
        string email,
        string password,
        string username,
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

        User user = new(UserId.New(), name, email, passwordHash, username);

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

    public void UpdateUserProfile(string? fullName, string? bio)
    {
        if (fullName is not null)
        {
            // this won't be necessary, if the Name field from Users table is dropped
            Name = fullName;
            UpdatedAt = DateTime.UtcNow;
        }
        UserProfile.UpdateDetails(fullName, bio);
    }

    public Result UpdateUserEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure(UserErrors.EmailEmpty);
        }
        Email = email;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result UpdateUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return Result.Failure(UserErrors.UsernameEmpty);
        }
        Username = username;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
