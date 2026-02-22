using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetAllCommentsOfPost;

public sealed record GetAllCommentsOfPostQuery(PostId PostId)
    : IQuery<IReadOnlyList<CommentResult>>;
