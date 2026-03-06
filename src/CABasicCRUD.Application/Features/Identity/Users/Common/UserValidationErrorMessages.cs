namespace CABasicCRUD.Application.Features.Identity.Users.Common;

public static class UserValidationErrorMessages
{
    public const string IdEmpty = "User Id cannot be empty.";
    public const string NameEmpty = "User full name is required.";
    public const string NameExceedsMaxCharacters = "User full name must not exceed 50 characters.";
    public const string EmailEmpty = "User email is required.";
    public const string BioEmpty = "Bio is required.";
    public const string BioExceedsMaxCharacters = "Bio must not exceed 300 characters.";
    public const string UsernameEmpty = "Username is required.";
    public const string UsernameExceedsMaxCharacters = "Username must not exceed 30 characters.";
}
