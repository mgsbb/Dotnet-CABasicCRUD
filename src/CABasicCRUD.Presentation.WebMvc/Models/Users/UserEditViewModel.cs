using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Users;

public class UserEditViewModel
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;
}
