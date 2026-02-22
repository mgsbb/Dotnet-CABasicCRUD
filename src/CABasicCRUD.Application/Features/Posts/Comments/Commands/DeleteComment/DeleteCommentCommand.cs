using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.DeleteComment;

public sealed record DeleteCommentCommand(CommentId Id, UserId UserId) : ICommand;
