using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.MemberItems;
using Nikcio.UHeadless.Members;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.Members;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MemberByUsernameQuery : MemberByUsernameQuery<MemberItem>
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
/// Implements the <see cref="MemberByUsername"/> query
/// </summary>
public abstract class MemberByUsernameQuery<TMemberItem> : IGraphQLQuery
    where TMemberItem : MemberItemBase
{
    public const string PolicyName = "MemberByUsernameQuery";

    public const string ClaimValue = "member.by.username.query";

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
    /// Gets a member by username
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets a member by username.")]
    public TMemberItem? MemberByUsername(
        IResolverContext resolverContext,
        [GraphQLDescription("The username to fetch.")] string username)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(username);

        IMemberItemRepository<TMemberItem> memberItemRepository = resolverContext.Service<IMemberItemRepository<TMemberItem>>();

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IMemberService memberService = resolverContext.Service<IMemberService>();

        IMember? member = memberService.GetByUsername(username);

        if (member == null)
        {
            ILogger<MemberByUsernameQuery> logger = resolverContext.Service<ILogger<MemberByUsernameQuery>>();
            logger.LogWarning("Member not found. {Username}", username);
            return default;
        }

        IPublishedContent? memberItem = memberCache.Get(member);

        return CreateMemberItem(memberItem, memberItemRepository, resolverContext);
    }

    protected abstract TMemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<TMemberItem> memberItemRepository, IResolverContext resolverContext);
}
