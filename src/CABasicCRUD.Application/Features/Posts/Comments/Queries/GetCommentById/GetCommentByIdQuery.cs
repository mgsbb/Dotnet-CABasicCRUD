using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetCommentById;

public sealed record GetCommentByIdQuery(CommentId Id) : IQuery<CommentResult>;
