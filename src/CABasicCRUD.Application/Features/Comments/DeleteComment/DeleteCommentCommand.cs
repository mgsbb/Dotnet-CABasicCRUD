using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;

namespace CABasicCRUD.Application.Features.Comments.DeleteComment;

public sealed record DeleteCommentCommand(CommentId Id) : ICommand;
