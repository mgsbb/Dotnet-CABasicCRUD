using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Common;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public record CreatePostCommand(CreatePostDTO CreatePostDTO) : IRequest<Result<PostDTO>>;
