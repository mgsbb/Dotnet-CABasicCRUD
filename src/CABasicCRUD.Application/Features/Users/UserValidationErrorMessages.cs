namespace CABasicCRUD.Application.Features.Users;

public static class UserValidationErrorMessages
{
    public const string IdEmpty = "User Id cannot be empty.";
    public const string NameEmpty = "User name is required.";
    public const string NameExceedsMaxCharacters = "User name must not exceed 50 characters.";
    public const string EmailEmpty = "User email is required.";
}
