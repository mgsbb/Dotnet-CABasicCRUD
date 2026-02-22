using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.DeletePost;

public sealed record DeletePostCommand(PostId PostId, UserId UserId) : ICommand;
