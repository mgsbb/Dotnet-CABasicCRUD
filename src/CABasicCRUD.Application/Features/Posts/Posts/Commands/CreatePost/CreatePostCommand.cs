using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.CreatePost;

public sealed record CreatePostCommand(
    string Title,
    string Content,
    UserId UserId,
    IReadOnlyList<CreatePostMedia> CreatePostMedia
) : ICommand<PostResult>;

public sealed record CreatePostMedia(
    Stream Stream,
    string FileName,
    MediaType MediaType,
    string ContentType
);
