using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(PostId PostId) : ICommand;
