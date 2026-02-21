using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Auth;

public class RegisterViewModel
{
    [Required, MaxLength(50, ErrorMessage = "Name must be at most 50 characters long.")]
    public string Name { get; set; } = default!;

    [
        Required,
        MinLength(5, ErrorMessage = "Username must be at least 5 characters long."),
        MaxLength(30, ErrorMessage = "Username must be at most 30 characters long.")
    ]
    public string Username { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [
        Required,
        MinLength(8, ErrorMessage = "Password must be at least 8 characters long."),
        MaxLength(128, ErrorMessage = "Password must be at most 128 characters long.")
    ]
    public string Password { get; set; } = default!;
}
