using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.UpdatePost;

public sealed record UpdatePostCommand(PostId PostId, string Title, string Content) : ICommand;
