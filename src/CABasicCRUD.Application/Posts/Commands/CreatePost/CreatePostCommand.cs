using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Posts.DTOs;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public record CreatePostCommand(CreatePostDTO CreatePostDTO) : ICommand<PostDTO>;
