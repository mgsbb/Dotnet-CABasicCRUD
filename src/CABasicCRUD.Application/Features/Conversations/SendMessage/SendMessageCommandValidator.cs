using FluentValidation;

namespace CABasicCRUD.Application.Features.Conversations.SendMessage;

public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(ConversationValidationErrorMessages.MessageEmpty)
            .MaximumLength(1000)
            .WithMessage(ConversationValidationErrorMessages.MessageTooLong);
    }
}
