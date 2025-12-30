using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;

namespace CABasicCRUD.Application.Features.Comments.GetCommentById;

public sealed record GetCommentByIdQuery(CommentId Id) : IQuery<CommentResult>;
