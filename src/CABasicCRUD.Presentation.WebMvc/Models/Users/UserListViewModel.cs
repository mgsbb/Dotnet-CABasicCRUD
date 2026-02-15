using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Presentation.WebMvc.Models.Users;

public sealed class UserListViewModel
{
    public string SearchTerm { get; set; } = default!;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public UserOrderBy OrderBy { get; init; } = UserOrderBy.CreatedAt;
    public SortDirection SortDirection { get; init; } = SortDirection.Desc;

    public IReadOnlyList<UserListItemViewModel> Users { get; init; } = [];
}
