using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Defaults.Auth;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a member picker
/// </summary>
[GraphQLDescription("Represents a member picker.")]
public class MemberPicker : MemberPicker<MemberPickerItem>
{
    public MemberPicker(CreateCommand command) : base(command)
    {
    }

    protected override MemberPickerItem CreateMemberPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new MemberPickerItem(publishedContent, resolverContext);
    }
}

/// <summary>
/// Represents a member picker
/// </summary>
[GraphQLDescription("Represents a member picker.")]
public abstract class MemberPicker<TMemberPickerItem> : PropertyValue
    where TMemberPickerItem : class
{
    public const string PolicyName = "PropertyValuesMemberPicker";

    public const string ClaimValue = "property.values.member.picker";

    /// <summary>
    /// The published members
    /// </summary>
    /// <value></value>
    protected List<IPublishedContent> PublishedMembers { get; }

    /// <summary>
    /// Gets the member items of a picker
    /// </summary>
    [Authorize(Policy = PolicyName)]
    [GraphQLDescription($"Gets the member items of a picker. Requires the {ClaimValue} or {DefaultClaimValues.GlobalMemberRead} claim to access")]
    public List<TMemberPickerItem> Members(IResolverContext resolverContext)
    {
        return PublishedMembers.Select(publishedMember =>
        {
            return CreateMemberPickerItem(publishedMember, resolverContext);
        }).ToList();
    }

    protected MemberPicker(CreateCommand command) : base(command)
    {
        IResolverContext resolverContext = command.ResolverContext;
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, resolverContext.Culture(), resolverContext.Segment(), resolverContext.Fallback());

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            PublishedMembers = [publishedContent];
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            PublishedMembers = publishedContentItems.ToList();
        }
        else
        {
            PublishedMembers = [];
        }
    }

    /// <summary>
    /// Creates a member picker item
    /// </summary>
    /// <param name="publishedContent"></param>
    /// <param name="resolverContext"></param>
    /// <returns></returns>
    protected abstract TMemberPickerItem CreateMemberPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext);

    internal static UHeadlessOptions ApplyConfiguration(UHeadlessOptions options)
    {
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

        return options;
    }
}

/// <summary>
/// Represents a member item
/// </summary>
[GraphQLDescription("Represents a member item.")]
public class MemberPickerItem
{
    /// <summary>
    /// The published content
    /// </summary>
    /// <value></value>
    protected IPublishedContent PublishedContent { get; }

    /// <summary>
    /// The culture of the query
    /// </summary>
    /// <value></value>
    protected string? Culture { get; }

    /// <summary>
    /// The variation context accessor
    /// </summary>
    /// <value></value>
    protected IVariationContextAccessor VariationContextAccessor { get; }

    /// <summary>
    /// The resolver context
    /// </summary>
    /// <value></value>
    protected IResolverContext ResolverContext { get; }

    /// <summary>
    /// Gets the name of a member item
    /// </summary>
    [GraphQLDescription("Gets the name of a member item.")]
    public string? Name => PublishedContent?.Name(VariationContextAccessor, Culture);

    /// <summary>
    /// Gets the id of a member item
    /// </summary>
    [GraphQLDescription("Gets the id of a member item.")]
    public int Id => PublishedContent.Id;

    /// <summary>
    /// Gets the key of a member item
    /// </summary>
    [GraphQLDescription("Gets the key of a member item.")]
    public Guid Key => PublishedContent.Key;

    /// <summary>
    /// Gets the properties of the member item
    /// </summary>
    [GraphQLDescription("Gets the properties of the member item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }

    public MemberPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        PublishedContent = publishedContent;
        ResolverContext = resolverContext;
        Culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        VariationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
