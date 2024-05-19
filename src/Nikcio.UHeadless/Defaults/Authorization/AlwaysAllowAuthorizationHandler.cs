using Microsoft.AspNetCore.Authorization;

namespace Nikcio.UHeadless.Defaults.Authorization;

public sealed class AlwaysAllowAuthoriaztionRequirement : IAuthorizationRequirement
{
}

internal sealed class AlwaysAllowAuthorizationHandler : AuthorizationHandler<AlwaysAllowAuthoriaztionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AlwaysAllowAuthoriaztionRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
