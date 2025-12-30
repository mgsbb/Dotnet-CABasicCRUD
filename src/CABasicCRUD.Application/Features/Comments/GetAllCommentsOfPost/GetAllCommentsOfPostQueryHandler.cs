using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments.GetAllCommentsOfPost;

internal sealed class GetAllCommentsOfPostQueryHandler(
    IPostRepository postRepository,
    ICommentRepository commentRepository
) : IQueryHander<GetAllCommentsOfPostQuery, IReadOnlyList<CommentResult>>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ICommentRepository _commentRepository = commentRepository;

    public async Task<Result<IReadOnlyList<CommentResult>>> Handle(
        GetAllCommentsOfPostQuery request,
        CancellationToken cancellationToken
    )
    {
        Post? post = await _postRepository.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<IReadOnlyList<CommentResult>>.Failure(Posts.PostErrors.NotFound);
        }

        IReadOnlyList<Comment> comments = await _commentRepository.GetAllCommentsOfPost(
            request.PostId
        );

        IReadOnlyList<CommentResult> commentResults = comments.ToListCommentResult();

        return Result<IReadOnlyList<CommentResult>>.Success(commentResults);
    }
}
