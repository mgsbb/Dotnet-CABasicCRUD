using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.GetPostByIdWithAuthor;

public sealed record GetPostByIdWithAuthorQuery(PostId PostId) : IQuery<PostWithAuthorResult>;
