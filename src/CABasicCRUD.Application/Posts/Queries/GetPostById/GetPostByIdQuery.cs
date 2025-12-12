using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Queries.GetPostById;

public record GetPostByIdQuery(PostId PostId) : IRequest<Result<PostDTO>>;
