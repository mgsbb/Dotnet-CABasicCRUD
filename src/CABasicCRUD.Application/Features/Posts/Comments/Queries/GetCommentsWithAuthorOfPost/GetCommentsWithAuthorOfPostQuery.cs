using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetCommentsWithAuthorOfPost;

public sealed record GetCommentsWithAuthorOfPostQuery(PostId PostId)
    : IQuery<IReadOnlyList<CommentWithAuthorResult>>;
