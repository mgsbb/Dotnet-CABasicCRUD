using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.CreatePost;

public sealed record CreatePostCommand(string Title, string Content, UserId UserId)
    : ICommand<PostResult>;
