using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Posts.Queries.GetPostById;

public record GetPostByIdQuery(PostId PostId) : IQuery<PostDTO>;
