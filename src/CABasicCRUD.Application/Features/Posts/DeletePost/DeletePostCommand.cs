using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.DeletePost;

public sealed record DeletePostCommand(PostId PostId) : ICommand;
