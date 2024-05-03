using Microsoft.AspNetCore.Authorization;

namespace Nikcio.UHeadless.Defaults;

internal class AlwaysAllowAuthoriaztionRequirement : IAuthorizationRequirement
{
}

internal class AlwaysAllowAuthorizationHandler : AuthorizationHandler<AlwaysAllowAuthoriaztionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AlwaysAllowAuthoriaztionRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
