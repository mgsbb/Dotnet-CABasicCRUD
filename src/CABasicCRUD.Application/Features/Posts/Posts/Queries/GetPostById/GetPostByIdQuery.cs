using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.GetPostById;

public sealed record GetPostByIdQuery(PostId PostId) : IQuery<PostResult>;
