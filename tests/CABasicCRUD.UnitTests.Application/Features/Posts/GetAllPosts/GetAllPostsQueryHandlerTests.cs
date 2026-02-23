using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Queries.GetAllPosts;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;
using NSubstitute;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.GetAllPosts;

public sealed class GetAllPostsQueryHandlerTests
{
    private readonly IPostReadService _postReadService;
    private readonly GetAllPostsQueryHandler _handler;
    private readonly ICacheService _cacheService;

    public GetAllPostsQueryHandlerTests()
    {
        _postReadService = Substitute.For<IPostReadService>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new GetAllPostsQueryHandler(_postReadService, _cacheService);
    }

    [Fact]
    public async Task Handle_WhenPostsExistsInCache_ShouldReturnsThePostsFromCache()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;

        GetAllPostsQuery query = new();
        CancellationToken token = default;

        _cacheService
            .GetAsync<IReadOnlyList<PostResult>>(Arg.Any<string>(), token)
            .Returns([post.ToPostResult()]);

        // Act
        Result<IReadOnlyList<PostResult>> result = await _handler.Handle(query, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        Assert.IsAssignableFrom<IReadOnlyList<PostResult>>(result.Value);
        Assert.NotEqual(result.Value, []);

        await _postReadService.DidNotReceive().GetAllAsync();
        await _cacheService
            .DidNotReceive()
            .SetAsync(
                Arg.Any<string>(),
                Arg.Any<IReadOnlyList<PostResult>>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task Handle_WhenPostsExistsButNotInCache_ShouldReturnsThePostsFromDatabaseAndSetCache()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;

        GetAllPostsQuery query = new();
        CancellationToken token = default;

        _postReadService.GetAllAsync().Returns([post]);
        _cacheService
            .GetAsync<IReadOnlyList<PostResult>>(Arg.Any<string>(), token)
            .Returns(null as IReadOnlyList<PostResult>);

        // Act
        Result<IReadOnlyList<PostResult>> result = await _handler.Handle(query, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        Assert.IsAssignableFrom<IReadOnlyList<PostResult>>(result.Value);
        Assert.NotEqual(result.Value, []);

        await _postReadService.Received(1).GetAllAsync();
        await _cacheService
            .Received(1)
            .SetAsync(
                Arg.Any<string>(),
                Arg.Any<IReadOnlyList<PostResult>>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task Handle_WhenNoPostsExists_ShouldReturnsEmptyList()
    {
        // // Arrange

        GetAllPostsQuery query = new();
        CancellationToken token = default;

        // is this necessary?
        _postReadService.GetAllAsync().Returns([]);
        _cacheService
            .GetAsync<IReadOnlyList<PostResult>>(Arg.Any<string>(), token)
            .Returns(null as IReadOnlyList<PostResult>);

        // Act
        Result<IReadOnlyList<PostResult>> result = await _handler.Handle(query, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        Assert.IsAssignableFrom<IReadOnlyList<PostResult>>(result.Value);
        Assert.Equal(result.Value, []);

        await _postReadService.Received(1).GetAllAsync();
        await _cacheService
            .Received(1)
            .SetAsync(
                Arg.Any<string>(),
                Arg.Any<IReadOnlyList<PostResult>>(),
                Arg.Any<CancellationToken>()
            );
    }
}
