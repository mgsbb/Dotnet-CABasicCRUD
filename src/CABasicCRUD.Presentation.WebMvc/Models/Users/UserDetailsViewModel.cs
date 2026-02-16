namespace CABasicCRUD.Presentation.WebMvc.Models.Users;

public sealed class UserDetailsViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
}
