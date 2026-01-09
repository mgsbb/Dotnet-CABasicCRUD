using FluentValidation;

namespace CABasicCRUD.Application.Features.Conversations.CreateConversation;

public sealed class CreateConversationCommandValidator
    : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ConversationValidationErrorMessages.IdEmpty);
    }
}
