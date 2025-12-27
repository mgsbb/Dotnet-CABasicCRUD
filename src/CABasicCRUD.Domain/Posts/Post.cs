using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Domain.Posts;

public class Post : EntityBase<PostId>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public UserId UserId { get; private set; }

    private Post(PostId id, string title, string content, UserId userId)
        : base(id)
    {
        Title = title;
        Content = content;
        UserId = userId;
    }

    public static Result<Post> Create(string? title, string? content, UserId userId)
    {
        if (string.IsNullOrEmpty(title))
        {
            return Result<Post>.Failure(PostErrors.TitleEmpty);
        }
        if (string.IsNullOrEmpty(content))
        {
            return Result<Post>.Failure(PostErrors.ContentEmpty);
        }
        Post post = new(PostId.New(), title, content, userId);
        return post;
    }

    public Result<Post> Update(string? title, string? content)
    {
        if (string.IsNullOrEmpty(title))
        {
            return Result<Post>.Failure(PostErrors.TitleEmpty);
        }
        if (string.IsNullOrEmpty(content))
        {
            return Result<Post>.Failure(PostErrors.ContentEmpty);
        }
        Title = title;
        Content = content;
        return this;
    }
}
