using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Queries.GetPostById;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;
using NSubstitute;
using PostErrors = CABasicCRUD.Application.Features.Posts.Posts.Common.PostErrors;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.GetPostById;

public sealed class GetPostByIdQueryHandlerTests
{
    private readonly IPostReadService _postReadService;
    private readonly GetPostByIdQueryHandler _handler;
    private readonly ICacheService _cacheService;

    public GetPostByIdQueryHandlerTests()
    {
        _postReadService = Substitute.For<IPostReadService>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new GetPostByIdQueryHandler(_postReadService, _cacheService);
    }

    [Fact]
    public async Task Handle_WhenPostExistsInCache_ShouldReturnThePostFromCache()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;
        GetPostByIdQuery query = new(post.Id);
        CancellationToken token = default;

        _cacheService.GetAsync<PostResult>(Arg.Any<string>()).Returns(post.ToPostResult());

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

        await _cacheService.Received(1).GetAsync<PostResult>(Arg.Any<string>());

        await _postReadService.DidNotReceive().GetByIdAsync(Arg.Any<PostId>());
        await _cacheService
            .DidNotReceive()
            .SetAsync(Arg.Any<string>(), Arg.Any<PostResult>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPostExistsButNotInCache_ShouldReturnThePostFromDatabaseAndSetCache()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;
        GetPostByIdQuery query = new(post.Id);
        CancellationToken token = default;

        _cacheService.GetAsync<PostResult>(Arg.Any<string>()).Returns(null as PostResult);
        _postReadService.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

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

        await _cacheService.Received(1).GetAsync<PostResult>(Arg.Any<string>());
        await _postReadService.Received(1).GetByIdAsync(Arg.Any<PostId>());
        await _cacheService
            .Received(1)
            .SetAsync(Arg.Any<string>(), Arg.Any<PostResult>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPostDoesNotExist_ShouldReturnNotFoundError()
    {
        // // Arrange
        GetPostByIdQuery query = new(PostId.New());
        CancellationToken token = default;

        _cacheService.GetAsync<PostResult>(Arg.Any<string>()).Returns(null as PostResult);
        _postReadService.GetByIdAsync(Arg.Any<PostId>()).Returns(null as Post);

        // Act
        Result<PostResult> result = await _handler.Handle(query, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, PostErrors.NotFound);

        await _cacheService.Received(1).GetAsync<PostResult>(Arg.Any<string>());

        await _postReadService.Received(1).GetByIdAsync(Arg.Any<PostId>());
        await _cacheService
            .DidNotReceive()
            .SetAsync(Arg.Any<string>(), Arg.Any<PostResult>(), Arg.Any<CancellationToken>());
    }
}
