using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Posts.DTOs;

namespace CABasicCRUD.Application.Posts.Queries.GetAllposts;

public record GetAllPostsQuery() : IQuery<IReadOnlyList<PostDTO>>;
