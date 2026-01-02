using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.GetPostById;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using NSubstitute;
using PostErrors = CABasicCRUD.Application.Features.Posts.PostErrors;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.GetPostById;

public sealed class GetPostByIdQueryHandlerTests
{
    private readonly IPostRepository _postRepository;
    private readonly GetPostByIdQueryHandler _handler;

    public GetPostByIdQueryHandlerTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _handler = new GetPostByIdQueryHandler(_postRepository);
    }

    [Fact]
    public async Task Handle_WhenPostExists_ShouldReturnThePost()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;
        GetPostByIdQuery query = new(post.Id);
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        // Act
        Result<PostResult> result = await _handler.Handle(query, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Value);
        Assert.Null(result.Error);
        Assert.Equal("title", result.Value.Title);
        Assert.Equal("content", result.Value.Content);
        Assert.IsType<PostResult>(result.Value);

        await _postRepository.Received(1).GetByIdAsync(Arg.Any<PostId>());
    }

    [Fact]
    public async Task Handle_WhenPostDoesNotExist_ShouldReturnNotFoundError()
    {
        // // Arrange
        GetPostByIdQuery query = new(PostId.New());
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(null as Post);

        // Act
        Result<PostResult> result = await _handler.Handle(query, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, PostErrors.NotFound);

        await _postRepository.Received(1).GetByIdAsync(Arg.Any<PostId>());
    }
}
