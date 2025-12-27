using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts.UpdatePost;

public sealed record UpdatePostCommand(PostId PostId, string Title, string Content, UserId UserId)
    : ICommand;
