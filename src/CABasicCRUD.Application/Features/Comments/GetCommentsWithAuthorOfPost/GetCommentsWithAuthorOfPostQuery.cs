using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments.GetCommentsWithAuthorOfPost;

public sealed record GetCommentsWithAuthorOfPostQuery(PostId PostId)
    : IQuery<IReadOnlyList<CommentWithAuthorResult>>;
