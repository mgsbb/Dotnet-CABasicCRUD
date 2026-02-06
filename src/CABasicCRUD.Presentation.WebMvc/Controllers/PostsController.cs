using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using CABasicCRUD.Application.Features.Posts.DeletePost;
using CABasicCRUD.Application.Features.Posts.GetAllposts;
using CABasicCRUD.Application.Features.Posts.GetPostById;
using CABasicCRUD.Application.Features.Posts.UpdatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Presentation.WebMvc.Models.Posts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

[Route("posts")]
public class PostsController : Controller
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        GetAllPostsQuery query = new();
        Result<IReadOnlyList<PostResult>> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return View(new List<PostListItemViewModel>());
        }

        IReadOnlyList<PostListItemViewModel> viewModel = result
            .Value.Select(p => new PostListItemViewModel { Id = p.Id, Title = p.Title })
            .ToList();

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

        PostDetailsViewModel viewModel = new()
        {
            Id = result.Value.Id,
            Title = result.Value.Title,
            Content = result.Value.Content,
            UserId = result.Value.UserId,
            CreatedAt = result.Value.CreatedAt,
            UpdatedAt = result.Value.UpdatedAt,
        };

        return View(viewModel);
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

        CreatePostCommand command = new(model.Title, model.Content);
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

        UpdatePostCommand command = new((PostId)id, model.Title, model.Content);
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
        DeletePostCommand command = new((PostId)id);
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
