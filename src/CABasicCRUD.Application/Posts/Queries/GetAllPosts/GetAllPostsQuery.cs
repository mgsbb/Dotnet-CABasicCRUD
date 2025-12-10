using CABasicCRUD.Application.Posts.DTOs;
using MediatR;

namespace CABasicCRUD.Application.Posts.Queries.GetAllposts;

public record GetAllPostsQuery() : IRequest<IReadOnlyList<PostDTO>>;
