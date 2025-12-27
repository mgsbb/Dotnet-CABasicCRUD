using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts.DeletePost;

public sealed record DeletePostCommand(PostId PostId, UserId UserId) : ICommand;
