namespace CABasicCRUD.Application.Features.Posts;

public static class PostValidationErrorMessages
{
    public const string TitleEmpty = "Post title cannot be null or whitespace.";
    public const string TitleExceedsMaxCharacters = "Post title cannot exceed 100 characters.";
    public const string ContentEmpty = "Post content cannot be null or whitespace.";
    public const string IdEmpty = "Post Id cannot be empty.";
}
