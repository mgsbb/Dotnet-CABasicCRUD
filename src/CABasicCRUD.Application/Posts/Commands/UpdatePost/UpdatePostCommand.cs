using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Posts.Commands.UpdatePost;

public record UpdatePostCommand(UpdatePostDTO UpdatePostDTO, PostId PostId) : ICommand;
