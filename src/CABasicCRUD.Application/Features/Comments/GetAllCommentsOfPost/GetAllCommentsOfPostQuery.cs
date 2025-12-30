using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments.GetAllCommentsOfPost;

public sealed record GetAllCommentsOfPostQuery(PostId PostId)
    : IQuery<IReadOnlyList<CommentResult>>;
