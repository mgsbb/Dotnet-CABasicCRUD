using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts;

public class Post : EntityBase<PostId>
{
    public string Title { get; private set; }
    public string Content { get; private set; }

    private Post(PostId id, string title, string content)
        : base(id)
    {
        Title = title;
        Content = content;
    }

    public static Post Create(string? title, string? content)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Post title cannot be null or empty", nameof(title));
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Post content cannot be null or empty", nameof(content));
        }
        return new(PostId.New(), title, content);
    }

    public void Update(string? title, string? content)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Post title cannot be null or empty", nameof(title));
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Post content cannot be null or empty", nameof(content));
        }
        Title = title;
        Content = content;
    }
}
