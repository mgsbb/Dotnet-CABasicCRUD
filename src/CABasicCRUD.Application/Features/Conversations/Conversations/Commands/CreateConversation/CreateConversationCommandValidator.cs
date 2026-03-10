using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Commands.CreateConversation;

public sealed class CreateConversationCommandValidator
    : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationCommandValidator()
    {
        RuleFor(x => x.CreatorUserId)
            .NotEmpty()
            .WithMessage(ConversationValidationErrorMessages.IdEmpty);

        RuleFor(x => x.GroupTitle)
            .NotEmpty()
            .WithMessage(ConversationValidationErrorMessages.IdEmpty)
            .When(x => x.GroupTitle is not null);

        RuleFor(x => x.ConversationType)
            .IsInEnum()
            .WithMessage(ConversationValidationErrorMessages.ConversationTypeInvalid);
    }
}
