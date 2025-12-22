using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Posts.DTOs;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public sealed record CreatePostCommand(string Title, string Content) : ICommand<PostResult>;
