using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Application.Features.Comments.CreateComment;
using CABasicCRUD.Application.Features.Comments.GetAllCommentsOfPost;
using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using CABasicCRUD.Application.Features.Posts.DeletePost;
using CABasicCRUD.Application.Features.Posts.GetPostById;
using CABasicCRUD.Application.Features.Posts.SearchPosts;
using CABasicCRUD.Application.Features.Posts.UpdatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebMvc.Models.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

[Authorize]
[Route("posts")]
public class PostsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public PostsController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(PostListViewModel model)
    {
        // if (!ModelState.IsValid)
        //     return RedirectToAction(nameof(Index));

        SearchPostsQuery query = new(
            model.SearchTerm,
            model.Page,
            model.PageSize,
            model.OrderBy,
            model.SortDirection
        );

        Result<IReadOnlyList<PostResult>> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return View(new List<PostListItemViewModel>());
        }

        IReadOnlyList<PostListItemViewModel> postListItems = result
            .Value.Select(p => new PostListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ContentPreview = string.Concat(p.Content.AsSpan(0, 100), "..."),
            })
            .ToList();

        PostListViewModel viewModel = new() { Posts = postListItems };

        return View(viewModel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        GetPostByIdQuery query = new((PostId)id);
        Result<PostResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }

        GetAllCommentsOfPostQuery commentsQuery = new((PostId)id);
        Result<IReadOnlyList<CommentResult>> commentsResult = await _mediator.Send(commentsQuery);

        if (commentsResult.IsFailure || commentsResult.Value is null)
        {
            return NotFound();
        }

        IReadOnlyList<CommentViewModel> commentViewModels = commentsResult
            .Value.Select(comment => new CommentViewModel
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
            })
            .ToList();

        PostDetailsViewModel viewModel = new()
        {
            Id = result.Value.Id,
            Title = result.Value.Title,
            Content = result.Value.Content,
            UserId = result.Value.UserId,
            CreatedAt = result.Value.CreatedAt,
            UpdatedAt = result.Value.UpdatedAt,
            Comments = commentViewModels,
        };

        return View(viewModel);
    }

    [HttpPost("/posts/{id}/comments")]
    public async Task<IActionResult> CreateComment(
        Guid id,
        [Bind(Prefix = "NewComment")] CommentCreateViewModel model
    )
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        CreateCommentCommand command = new(model.Body, (PostId)id, (UserId)_currentUser.UserId);
        Result<CommentResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    public async Task<IActionResult> Create(PostCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        CreatePostCommand command = new(model.Title, model.Content, (UserId)_currentUser.UserId);
        Result<PostResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            if (result.Error == null)
                throw new InvalidOperationException();

            ModelState.AddModelError(string.Empty, result.Error.ToString());
            return View(model);
        }

        return RedirectToAction("Index");
        // return RedirectToAction(nameof(Details), new { id = result.Value.Id.Value });
    }

    [HttpGet("{id}/edit")]
    public IActionResult Edit() => View();

    [HttpPost("{id}/edit")]
    public async Task<IActionResult> Edit(PostEditViewModel model, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        UpdatePostCommand command = new(
            (PostId)id,
            model.Title,
            model.Content,
            (UserId)_currentUser.UserId
        );
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error == null)
                throw new InvalidOperationException();

            ModelState.AddModelError(string.Empty, result.Error.ToString());
            return View(model);
        }

        return RedirectToAction("Index");
        // return RedirectToAction(nameof(Details), new { id = result.Value.Id.Value });
    }

    [HttpGet("{id}/delete")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        GetPostByIdQuery query = new((PostId)id);
        Result<PostResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
            return NotFound();

        var viewModel = new PostDeleteViewModel
        {
            Id = result.Value.Id,
            Title = result.Value.Title,
        };

        return View(viewModel);
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        DeletePostCommand command = new((PostId)id, (UserId)_currentUser.UserId);
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
