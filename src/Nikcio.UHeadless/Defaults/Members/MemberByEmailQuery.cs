using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Defaults.Auth;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.MemberItems;
using Nikcio.UHeadless.Members;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;

namespace Nikcio.UHeadless.Defaults.Members;

[ExtendObjectType(typeof(HotChocolateQueryObject))]
public class MemberByEmailQuery : MemberByEmailQuery<MemberItem>
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
/// Implements the <see cref="MemberByEmail"/> query
/// </summary>
public abstract class MemberByEmailQuery<TMemberItem> : IGraphQLQuery
    where TMemberItem : MemberItemBase
{
    public const string PolicyName = "MemberByEmailQuery";

    public const string ClaimValue = "member.by.email.query";

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
    /// Gets a member by email
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Gets a member by email.")]
    public virtual TMemberItem? MemberByEmail(
        IResolverContext resolverContext,
        [GraphQLDescription("The email to fetch.")] string email)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(email);

        IMemberItemRepository<TMemberItem> memberItemRepository = resolverContext.Service<IMemberItemRepository<TMemberItem>>();

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IMemberService memberService = resolverContext.Service<IMemberService>();

        IMember? member = memberService.GetByEmail(email);

        if (member == null)
        {
            ILogger<MemberByEmailQuery> logger = resolverContext.Service<ILogger<MemberByEmailQuery>>();
            logger.LogInformation("Member not found with given email.");
            return default;
        }

        IPublishedContent? memberItem = memberCache.Get(member);

        return CreateMemberItem(memberItem, memberItemRepository, resolverContext);
    }

    protected abstract TMemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<TMemberItem> memberItemRepository, IResolverContext resolverContext);
}
