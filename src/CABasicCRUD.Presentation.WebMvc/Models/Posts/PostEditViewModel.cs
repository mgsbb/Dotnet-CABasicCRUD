using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public class PostEditViewModel
{
    [MinLength(5), MaxLength(100)]
    public string? Title { get; set; } = default!;

    [MinLength(10)]
    public string? Content { get; set; } = default!;
}
