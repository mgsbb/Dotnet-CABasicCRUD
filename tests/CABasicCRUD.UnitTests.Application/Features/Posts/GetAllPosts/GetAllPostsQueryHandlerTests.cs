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

    public GetAllPostsQueryHandlerTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _handler = new GetAllPostsQueryHandler(_postRepository);
    }

    [Fact]
    public async Task Handle_WhenPostsExists_ShouldReturnsThePosts()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;

        GetAllPostsQuery query = new();
        CancellationToken token = default;

        _postRepository.GetAllAsync().Returns([post]);

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
    }

    [Fact]
    public async Task Handle_WhenNoPostsExists_ShouldReturnsEmptyList()
    {
        // // Arrange

        GetAllPostsQuery query = new();
        CancellationToken token = default;

        // is this necessary?
        _postRepository.GetAllAsync().Returns([]);

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
    }
}
