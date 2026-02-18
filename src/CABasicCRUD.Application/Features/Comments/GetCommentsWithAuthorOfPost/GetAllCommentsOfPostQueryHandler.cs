using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments.GetCommentsWithAuthorOfPost;

internal sealed class GetAllCommentsOfPostQueryHandler(
    IPostRepository postRepository,
    ICommentReadService commentReadService
) : IQueryHander<GetCommentsWithAuthorOfPostQuery, IReadOnlyList<CommentWithAuthorResult>>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ICommentReadService _commentReadService = commentReadService;

    public async Task<Result<IReadOnlyList<CommentWithAuthorResult>>> Handle(
        GetCommentsWithAuthorOfPostQuery request,
        CancellationToken cancellationToken
    )
    {
        Post? post = await _postRepository.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<IReadOnlyList<CommentWithAuthorResult>>.Failure(
                Posts.PostErrors.NotFound
            );
        }

        IReadOnlyList<CommentWithAuthorResult> comments =
            await _commentReadService.GetCommentsWithAuthorOfPostAsync(
                request.PostId,
                cancellationToken
            );

        return Result<IReadOnlyList<CommentWithAuthorResult>>.Success(comments);
    }
}
