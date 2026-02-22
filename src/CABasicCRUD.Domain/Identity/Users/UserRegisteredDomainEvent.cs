using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Identity.Users;

public sealed record UserRegisteredDomainEvent(UserId UserId, string Name, string Email)
    : IDomainEvent;
