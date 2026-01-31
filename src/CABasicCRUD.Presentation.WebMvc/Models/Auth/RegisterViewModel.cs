using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Auth;

public class RegisterViewModel
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(8), MaxLength(128)]
    public string Password { get; set; } = default!;
}
