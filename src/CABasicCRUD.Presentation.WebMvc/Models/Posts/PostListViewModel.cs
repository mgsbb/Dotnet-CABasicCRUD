using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public sealed class PostListViewModel
{
    public string SearchTerm { get; set; } = default!;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public PostOrderBy OrderBy { get; init; } = PostOrderBy.CreatedAt;
    public SortDirection SortDirection { get; init; } = SortDirection.Desc;

    public IReadOnlyList<PostListItemViewModel> Posts { get; init; } = [];
}
