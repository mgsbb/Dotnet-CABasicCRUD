using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments.DeleteComment;

public sealed record DeleteCommentCommand(CommentId Id, UserId UserId) : ICommand;
