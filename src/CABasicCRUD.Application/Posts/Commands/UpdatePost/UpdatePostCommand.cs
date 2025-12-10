using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.UpdatePost;

public record UpdatePostCommand(UpdatePostDTO UpdatePostDTO, PostId PostId) : IRequest;
