using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.UpdatePost;

public sealed record UpdatePostCommand(PostId PostId, string Title, string Content, UserId UserId)
    : ICommand;
