using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public class PostEditViewModel
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = default!;

    [Required]
    public string Content { get; set; } = default!;
}
