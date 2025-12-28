using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Features.Posts.CreatePost;

public sealed record CreatePostCommand(string Title, string Content) : ICommand<PostResult>;
