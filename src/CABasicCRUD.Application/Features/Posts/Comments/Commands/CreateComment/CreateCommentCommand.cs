using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.CreateComment;

public sealed record CreateCommentCommand(string Body, PostId PostId, UserId UserId)
    : ICommand<CommentResult>;
