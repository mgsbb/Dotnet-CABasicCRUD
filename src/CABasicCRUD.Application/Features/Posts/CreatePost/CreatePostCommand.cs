using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts.CreatePost;

public sealed record CreatePostCommand(string Title, string Content, UserId UserId)
    : ICommand<PostResult>;
