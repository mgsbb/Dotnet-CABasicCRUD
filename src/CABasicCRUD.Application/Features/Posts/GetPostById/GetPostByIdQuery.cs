using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.GetPostById;

public sealed record GetPostByIdQuery(PostId PostId) : IQuery<PostResult>;
