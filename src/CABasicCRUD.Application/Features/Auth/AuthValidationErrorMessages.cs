namespace CABasicCRUD.Application.Features.Auth;

public static class AuthValidationErrorMessages
{
    public const string NameEmpty = "User name is required.";
    public const string NameExceedsMaxCharacters = "User name must not exceed 50 characters.";
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
