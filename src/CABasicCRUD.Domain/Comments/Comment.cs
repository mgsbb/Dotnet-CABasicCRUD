using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Domain.Comments;

public sealed class Comment : EntityBase<CommentId>
{
    public string Body { get; private set; }
    public PostId PostId { get; private set; }
    public UserId UserId { get; private set; }

    private Comment(CommentId id, string body, PostId postId, UserId userId)
        : base(id)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }

    public static Result<Comment> Create(string? body, PostId postId, UserId userId)
    {
        if (string.IsNullOrEmpty(body))
        {
            return Result<Comment>.Failure(CommentErrors.BodyEmpty);
        }

        Comment comment = new(CommentId.New(), body, postId, userId);
        return comment;
    }

    public Result<Comment> Update(string? body)
    {
        if (string.IsNullOrEmpty(body))
        {
            return Result<Comment>.Failure(CommentErrors.BodyEmpty);
        }
        Body = body;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}
