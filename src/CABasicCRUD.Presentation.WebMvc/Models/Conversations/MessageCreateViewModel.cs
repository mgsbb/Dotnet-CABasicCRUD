using System.ComponentModel.DataAnnotations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Conversations;

public sealed class MessageCreateViewModel
{
    [Required]
    public string Content { get; set; } = default!;
}
