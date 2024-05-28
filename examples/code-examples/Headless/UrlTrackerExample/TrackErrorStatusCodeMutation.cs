using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Defaults.Authorization;
using UrlTracker.Middleware.Background;

namespace Code.Examples.Headless.UrlTrackerExample;

[ExtendObjectType(typeof(HotChocolateMutationObject))]
public class TrackErrorStatusCodeMutation : IGraphQLMutation
{
    public const string PolicyName = "TrackErrorStatusCode";

    public const string ClaimValue = "track.error.statuscode.mutation";

    [GraphQLIgnore]
    public virtual void ApplyConfiguration(UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.UmbracoBuilder.Services.AddAuthorizationBuilder().AddPolicy(PolicyName, policy =>
        {
            if (options.DisableAuthorization)
            {
                policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
                return;
            }

            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadless);

            policy.RequireAuthenticatedUser();

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue);
        });
    }

    public async Task<TrackErrorStatusCodeResponse> TrackErrorStatusCodeAsync(
        IResolverContext resolverContext,
        [GraphQLDescription("The URL that generated the client error.")] string url,
        [GraphQLDescription("The time and date at which the client error was generated")] DateTime timestamp,
        [GraphQLDescription("The URL from which the current URL is requested")] string? referrer)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IClientErrorProcessorQueue clientErrorProcessorQueue = resolverContext.Service<IClientErrorProcessorQueue>();
        await clientErrorProcessorQueue.WriteAsync(new ClientErrorProcessorItem(url, timestamp, referrer)).ConfigureAwait(false);

        return new TrackErrorStatusCodeResponse
        {
            Success = true
        };
    }
}

public sealed class TrackErrorStatusCodeResponse
{
    public required bool Success { get; init; }
}
