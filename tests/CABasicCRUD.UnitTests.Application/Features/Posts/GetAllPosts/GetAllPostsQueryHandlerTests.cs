using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.GetAllposts;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using NSubstitute;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.GetAllPosts;

public sealed class GetAllPostsQueryHandlerTests
{
    private readonly IPostRepository _postRepository;
    private readonly GetAllPostsQueryHandler _handler;
    private readonly ICacheService _cacheService;

    public GetAllPostsQueryHandlerTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new GetAllPostsQueryHandler(_postRepository, _cacheService);
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

        await _postRepository.DidNotReceive().GetAllAsync();
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

        _postRepository.GetAllAsync().Returns([post]);
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

        await _postRepository.Received(1).GetAllAsync();
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
        _postRepository.GetAllAsync().Returns([]);
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

        await _postRepository.Received(1).GetAllAsync();
        await _cacheService
            .Received(1)
            .SetAsync(
                Arg.Any<string>(),
                Arg.Any<IReadOnlyList<PostResult>>(),
                Arg.Any<CancellationToken>()
            );
    }
}
