using CABasicCRUD.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CABasicCRUD.Presentation.WebAPI.Common.Security.Authorization.ResourceOwner;

public sealed class ResourceOwnerHandler(ICurrentUser currentUser)
    : AuthorizationHandler<ResourceOwnerRequirement, Guid>
{
    private readonly ICurrentUser _currentUser = currentUser;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOwnerRequirement requirement,
        Guid resourceUserId
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Task.CompletedTask;
        }

        if (_currentUser.UserId == resourceUserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
