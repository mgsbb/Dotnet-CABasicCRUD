using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Auth.Events;

public sealed class UserRegisteredDomainEventHandler
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly IEmailSender _emailSender;

    public UserRegisteredDomainEventHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(
        UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken
    )
    {
        await _emailSender.SendWelcomeEmailAsync(
            domainEvent.Name,
            domainEvent.Email,
            cancellationToken
        );
    }
}
