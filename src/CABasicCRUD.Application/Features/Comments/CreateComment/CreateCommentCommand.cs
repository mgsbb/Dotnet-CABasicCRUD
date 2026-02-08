using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments.CreateComment;

public sealed record CreateCommentCommand(string Body, PostId PostId, UserId UserId)
    : ICommand<CommentResult>;
