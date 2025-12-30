using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments.UpdateComment;

public sealed record UpdateCommentCommand(CommentId Id, string Body) : ICommand;
