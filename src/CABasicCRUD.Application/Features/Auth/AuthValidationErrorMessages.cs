namespace CABasicCRUD.Application.Features.Auth;

public static class AuthValidationErrorMessages
{
    public const string NameEmpty = "User fullname is required.";
    public const string NameExceedsMaxCharacters = "User fullname must not exceed 50 characters.";
    public const string UsernameEmtpy = "Username is required.";
    public const string UsernameExceedsMaxCharacters = "Username must not exceed 30 characters.";
    public const string UsernameLessThanMinCharacters = "Username must be at least 5 characters.";
    public const string EmailEmpty = "User email is required.";
    public const string PasswordEmpty = "User password is required.";
    public const string PasswordMinCharacters = "Password must be at least 8 characters long.";
    public const string PasswordMaxCharacters = "Password  must not exceed 128 characters.";
    public const string PasswordUppercase = "Password must contain at least one uppercase letter.";
    public const string PasswordLowercase = "Password must contain at least one lowercase letter.";
    public const string PasswordDigit = "Password must contain at least one digit.";
    public const string PasswordSpecial =
        @"Password must contain at least one special character from [!@#$%^&*(),.?""{}|<>_\-+=].";
    public const string PasswordWhitespace = "Password must not start or end with whitespace.";
}
