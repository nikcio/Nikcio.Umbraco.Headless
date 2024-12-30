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
public class FindMembersByEmailQuery : FindMembersByEmailQuery<MemberItem>
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
/// Implements the <see cref="FindMembersByEmail"/> query
/// </summary>
public abstract class FindMembersByEmailQuery<TMemberItem> : IGraphQLQuery
    where TMemberItem : MemberItemBase
{
    public const string PolicyName = "FindMembersByEmailQuery";

    public const string ClaimValue = "find.members.by.email.query";

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
    /// Finds members by email
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription("Finds members by email.")]
    public virtual List<TMemberItem?> FindMembersByEmail(
        IResolverContext resolverContext,
        [GraphQLDescription("The email (may be partial).")] string email,
        [GraphQLDescription("Determines how to match a string property value.")] StringPropertyMatchType matchType,
        [GraphQLDescription("The page number to fetch. Defaults to 1.")] int page = 1,
        [GraphQLDescription("How many items to include in a page. Defaults to 10.")] int pageSize = 10)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentException.ThrowIfNullOrEmpty(email);

        // We normalize the page to be 0-based because the repository is 0-based
        page--;

        if (page < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "The page must be greater than or equal to one");
        }

        if (pageSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size must be greater than zero");
        }

        IMemberItemRepository<TMemberItem> memberItemRepository = resolverContext.Service<IMemberItemRepository<TMemberItem>>();

        IPublishedMemberCache? memberCache = memberItemRepository.GetCache();

        if (memberCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IMemberService memberService = resolverContext.Service<IMemberService>();

        IEnumerable<IMember> members = memberService.FindByEmail(email, page, pageSize, out long totalRecords, matchType);

        IEnumerable<IPublishedContent?> memberItems = members.Select(memberCache.Get);

        return memberItems.Select(member => CreateMemberItem(member, memberItemRepository, resolverContext)).ToList();
    }

    protected abstract TMemberItem? CreateMemberItem(IPublishedContent? member, IMemberItemRepository<TMemberItem> memberItemRepository, IResolverContext resolverContext);
}
