using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using NSubstitute;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.CreatePost;

public sealed class CreatePostCommandHandlerTests
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly CreatePostCommandHandler _handler;

    private readonly ICacheService _cacheService;

    public CreatePostCommandHandlerTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _currentUser = Substitute.For<ICurrentUser>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new CreatePostCommandHandler(
            _postRepository,
            _unitOfWork,
            _currentUser,
            _cacheService
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthenticatedAndInputIsValid_ShouldCreatePostSuccessfully()
    {
        // // Arrange
        UserId userId = UserId.New();
        CreatePostCommand command = new("title", "content");
        CancellationToken token = default;

        _currentUser.IsAuthenticated.Returns(true);
        _currentUser.UserId.Returns(userId.Value);

        Post expectedPost = Post.Create("title", "content", userId).Value!;
        _postRepository.AddAsync(Arg.Any<Post>()).Returns(expectedPost);

        // Act
        Result<PostResult> result = await _handler.Handle(command, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Value);
        Assert.Null(result.Error);
        Assert.Equal("title", result.Value.Title);
        Assert.Equal("content", result.Value.Content);
        Assert.IsType<PostResult>(result.Value);

        await _postRepository
            .Received(1)
            .AddAsync(
                Arg.Is<Post>(p =>
                    p.Title == "title" && p.Content == "content" && p.UserId == userId
                )
            );
        await _unitOfWork.Received(1).SaveChangesAsync(token);
        await _cacheService.Received(1).RemoveAsync("posts:all", token);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_ShouldReturnAuthenticationError()
    {
        // // Arrange
        CreatePostCommand command = new("title", "content");
        CancellationToken token = default;
        _currentUser.IsAuthenticated.Returns(false);

        // Act
        Result<PostResult> result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Equal(result.Error, AuthErrors.Unauthenticated);

        await _postRepository.DidNotReceive().AddAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPostTitleIsEmpty_ShouldReturnFailure()
    {
        // Arrange
        UserId userId = UserId.New();
        CreatePostCommand command = new("", "content");
        CancellationToken token = default;

        _currentUser.IsAuthenticated.Returns(true);
        _currentUser.UserId.Returns(userId.Value);

        // Act
        Result<PostResult> result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, Domain.Posts.PostErrors.TitleEmpty);

        await _postRepository.DidNotReceive().AddAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPostContentIsEmpty_ShouldReturnFailure()
    {
        // Arrange
        UserId userId = UserId.New();
        CreatePostCommand command = new("title", "");
        CancellationToken token = default;

        _currentUser.IsAuthenticated.Returns(true);
        _currentUser.UserId.Returns(userId.Value);

        // Act
        Result<PostResult> result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, Domain.Posts.PostErrors.ContentEmpty);

        await _postRepository.DidNotReceive().AddAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
