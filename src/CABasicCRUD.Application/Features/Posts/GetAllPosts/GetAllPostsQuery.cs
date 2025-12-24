using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Features.Posts.GetAllposts;

public sealed record GetAllPostsQuery() : IQuery<IReadOnlyList<PostResult>>;
