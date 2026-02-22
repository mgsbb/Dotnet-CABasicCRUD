using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.GetPostByIdWithAuthor;

public sealed record GetPostByIdWithAuthorQuery(PostId PostId) : IQuery<PostWithAuthorResult>;
