using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public sealed class CommentCreateViewModel
{
    [Required]
    public string Body { get; init; } = default!;
}
