using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Mapping;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using MediatR;
using PostErrors = CABasicCRUD.Application.Posts.Errors.PostErrors;

namespace CABasicCRUD.Application.Posts.Queries.GetPostById;

public class GetPostByIdQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostByIdQuery, Result<PostDTO>>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<Result<PostDTO>> Handle(
        GetPostByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result<PostDTO>.Failure(PostErrors.NotFound);
        }

        var postDTO = post.ToPostDTO();

        return postDTO;
    }
}
