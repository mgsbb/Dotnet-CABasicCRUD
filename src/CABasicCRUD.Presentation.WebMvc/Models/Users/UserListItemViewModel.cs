namespace CABasicCRUD.Presentation.WebMvc.Models.Users;

public class UserListItemViewModel
{
    public Guid Id { get; init; } = default!;

    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
}
