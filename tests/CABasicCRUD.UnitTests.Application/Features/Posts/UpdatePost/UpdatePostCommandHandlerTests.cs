using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Posts.UpdatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using NSubstitute;
using PostErrors = CABasicCRUD.Application.Features.Posts.PostErrors;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.UpdatePost;

public sealed class UpdatePostCommandHandlerTests
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdatePostCommandHandler _handler;
    private readonly ICacheService _cacheService;

    public UpdatePostCommandHandlerTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new UpdatePostCommandHandler(_postRepository, _unitOfWork, _cacheService);
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthorizedAndInputIsValid_ShouldUpdatePostSuccessfully()
    {
        // // Arrange
        UserId userId = UserId.New();
        Post post = Post.Create("title", "content", userId).Value!;
        UpdatePostCommand command = new(post.Id, "title_updated", "content_updated", userId);
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);

        await _postRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<Post>(p =>
                    p.Title == "title_updated"
                    && p.Content == "content_updated"
                    && p.UserId == userId
                )
            );
        await _unitOfWork.Received(1).SaveChangesAsync(token);

        await _cacheService.Received(2).RemoveAsync(Arg.Any<string>(), token);
    }

    [Fact]
    public async Task Handle_PostDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        UpdatePostCommand command = new(PostId.New(), "title", "content", UserId.New());
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(null as Post);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, PostErrors.NotFound);

        await _postRepository.DidNotReceive().UpdateAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_PostDoesNotBelongToUser_ShouldReturnForbiddenError()
    {
        // Arrange
        UserId ownerId = UserId.New();
        UserId anotherUserId = UserId.New();
        Post post = Post.Create("title", "content", ownerId).Value!;
        UpdatePostCommand command = new(post.Id, "title", "content", UserId.New());
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, AuthErrors.Forbidden);

        await _postRepository.DidNotReceive().UpdateAsync(Arg.Any<Post>());
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
        Post post = Post.Create("title", "content", userId).Value!;
        UpdatePostCommand command = new(post.Id, "", "content", userId);
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, Domain.Posts.PostErrors.TitleEmpty);

        await _postRepository.DidNotReceive().UpdateAsync(Arg.Any<Post>());
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
        Post post = Post.Create("title", "content", userId).Value!;
        UpdatePostCommand command = new(post.Id, "title", "", userId);
        CancellationToken token = default;

        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        // Act
        Result result = await _handler.Handle(command, token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error, Domain.Posts.PostErrors.ContentEmpty);

        await _postRepository.DidNotReceive().UpdateAsync(Arg.Any<Post>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _cacheService
            .DidNotReceive()
            .RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
