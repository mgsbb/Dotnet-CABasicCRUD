using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Commands.CreateConversation;

public sealed class CreateConversationCommandValidator
    : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ConversationValidationErrorMessages.IdEmpty);
    }
}
