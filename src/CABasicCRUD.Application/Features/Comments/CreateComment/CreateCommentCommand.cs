using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments.CreateComment;

public sealed record CreateCommentCommand(string Body, PostId PostId) : ICommand<CommentResult>;
