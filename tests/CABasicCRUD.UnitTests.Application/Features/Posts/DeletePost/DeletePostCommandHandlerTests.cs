using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Posts.DeletePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using NSubstitute;
using PostErrors = CABasicCRUD.Application.Features.Posts.PostErrors;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.DeletePost;

public sealed class DeletePostCommandHandlerTests
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly DeletePostCommandHandler _handler;
    private readonly ICacheService _cacheService;

    public DeletePostCommandHandlerTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _currentUser = Substitute.For<ICurrentUser>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new DeletePostCommandHandler(
            _postRepository,
            _unitOfWork,
            _currentUser,
            _cacheService
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthenticatedAndPostExists_ShouldDeletePostSuccessfully()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post postToDelete = Post.Create("title", "content", userId).Value!;
        PostId postId = postToDelete.Id;

        DeletePostCommand command = new(postId);
        CancellationToken token = default;

        _currentUser.IsAuthenticated.Returns(true);
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(postToDelete);
        _currentUser.UserId.Returns(userId.Value);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);

        await _postRepository
            .Received(1)
            .DeleteAsync(
                Arg.Is<Post>(p =>
                    p.Title == "title" && p.Content == "content" && p.UserId == userId
                )
            );
        await _unitOfWork.Received(1).SaveChangesAsync(token);

        // 2 calls - for cacheKey, posts:all
        await _cacheService.Received(2).RemoveAsync(Arg.Any<string>(), token);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_ShouldReturnAuthenticationError()
    {
        // // Arrange
        PostId postId = PostId.New();
        DeletePostCommand command = new(postId);
        CancellationToken token = default;
        _currentUser.IsAuthenticated.Returns(false);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(result.Error, AuthErrors.Unauthenticated);

        await _postRepository.DidNotReceive().DeleteAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthenticatedButPostDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        PostId postId = PostId.New();
        DeletePostCommand command = new(postId);
        CancellationToken token = default;
        _currentUser.IsAuthenticated.Returns(true);
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(null as Post);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, PostErrors.NotFound);

        await _postRepository.DidNotReceive().AddAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPostDoesNotBelongToUser_ShouldReturnForbiddenError()
    {
        // Arrange
        UserId differentUserId = UserId.New();
        UserId postOwnerId = UserId.New();
        Post postToDelete = Post.Create("title", "content", postOwnerId).Value!;
        DeletePostCommand command = new(postToDelete.Id);
        CancellationToken token = default;
        _currentUser.IsAuthenticated.Returns(true);
        _currentUser.UserId.Returns(differentUserId.Value);
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(postToDelete);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, AuthErrors.Forbidden);

        await _postRepository.DidNotReceive().AddAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
