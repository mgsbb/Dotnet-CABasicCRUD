using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Posts.DTOs;

namespace CABasicCRUD.Application.Posts.Queries.GetAllposts;

public sealed record GetAllPostsQuery() : IQuery<IReadOnlyList<PostResult>>;
