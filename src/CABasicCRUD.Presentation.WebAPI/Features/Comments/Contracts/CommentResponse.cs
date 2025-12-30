namespace CABasicCRUD.Presentation.WebAPI.Features.Comments.Contracts;

public record CommentResponse(Guid Id, string Body, Guid PostId, Guid UserId);
