using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.GetAllposts;
using CABasicCRUD.Domain.Common;
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
}
