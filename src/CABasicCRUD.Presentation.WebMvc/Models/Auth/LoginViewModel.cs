using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Auth;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}
