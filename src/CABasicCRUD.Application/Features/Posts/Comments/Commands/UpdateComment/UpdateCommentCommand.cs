using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.UpdateComment;

public sealed record UpdateCommentCommand(CommentId Id, string Body, UserId UserId) : ICommand;
