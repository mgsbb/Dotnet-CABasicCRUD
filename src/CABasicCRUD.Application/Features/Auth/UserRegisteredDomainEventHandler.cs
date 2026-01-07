using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Auth;

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
        throw new Exception("Deliberate exception");
        await _emailSender.SendWelcomeEmailAsync(
            domainEvent.Name,
            domainEvent.Email,
            cancellationToken
        );
    }
}
