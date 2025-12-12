using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Common;
using MediatR;

namespace CABasicCRUD.Application.Posts.Queries.GetAllposts;

public record GetAllPostsQuery() : IRequest<Result<IReadOnlyList<PostDTO>>>;
