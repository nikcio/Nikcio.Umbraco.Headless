using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.MemberItems;
using Nikcio.UHeadless.Members;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.Members;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class FindMembersByRoleQuery : FindMembersByRoleQuery<MemberItem>
{
    protected override MemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<MemberItem> memberItemRepository, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(memberItemRepository);

        return memberItemRepository.GetMemberItem(new MemberItemBase.CreateCommand()
        {
            PublishedContent = member,
            ResolverContext = resolverContext,
        });
    }
}

/// <summary>
/// Implements the <see cref="FindMembersByRole"/> query
/// </summary>
public abstract class FindMembersByRoleQuery<TMemberItem> : IGraphQLQuery
    where TMemberItem : MemberItemBase
{
    public const string PolicyName = "FindMembersByRoleQuery";

    public const string ClaimValue = "find.members.by.role.query";

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

            policy.RequireClaim(DefaultClaims.UHeadlessScope, ClaimValue, DefaultClaimValues.GlobalMemberRead);
        });

        AvailableClaimValue availableClaimValue = new()
        {
            Name = DefaultClaims.UHeadlessScope,
            Values = [ClaimValue, DefaultClaimValues.GlobalMemberRead]
        };
        AuthorizationTokenProvider.AddAvailableClaimValue(ClaimValueGroups.Members, availableClaimValue);
    }

    /// <summary>
    /// Finds members by role
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Finds members by role.")]
    public virtual PaginationResult<TMemberItem?> FindMembersByRole(
        IResolverContext resolverContext,
        [GraphQLDescription("The role name.")] string roleName,
        [GraphQLDescription("The username to match.")] string usernameToMatch,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        IMemberItemRepository<TMemberItem> memberItemRepository = resolverContext.Service<IMemberItemRepository<TMemberItem>>();

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IMemberService memberService = resolverContext.Service<IMemberService>();

        IEnumerable<IMember> members = memberService.FindMembersInRole(roleName, usernameToMatch, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        IEnumerable<TMemberItem?> resultItems = memberItems.Select(member => CreateMemberItem(member, memberItemRepository, resolverContext));

        return new PaginationResult<TMemberItem?>(resultItems, page, pageSize);
    }

    protected abstract TMemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<TMemberItem> memberItemRepository, IResolverContext resolverContext);
}
