using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Users;

public class UserEditViewModel
{
    [MaxLength(50)]
    public string? FullName { get; set; } = default!;

    public string? Bio { get; set; } = default!;

    [EmailAddress]
    public string? Email { get; set; } = default!;

    public string? Password { get; set; } = default!;
}
