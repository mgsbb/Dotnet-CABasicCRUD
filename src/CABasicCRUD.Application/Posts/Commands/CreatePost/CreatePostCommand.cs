using CABasicCRUD.Application.Posts.DTOs;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public record CreatePostCommand(CreatePostDTO CreatePostDTO) : IRequest<PostDTO>;
