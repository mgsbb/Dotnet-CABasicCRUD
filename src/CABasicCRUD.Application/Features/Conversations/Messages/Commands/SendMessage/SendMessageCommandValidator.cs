using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Conversations.Messages.Commands.SendMessage;

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
