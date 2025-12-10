using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(PostId PostId) : IRequest;
