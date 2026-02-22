using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.GetAllPosts;

public sealed record GetAllPostsQuery() : IQuery<IReadOnlyList<PostResult>>;
