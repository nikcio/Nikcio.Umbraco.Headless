using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common;
using Nikcio.UHeadless.Common.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Properties;

/// <summary>
/// Represents a member picker
/// </summary>
[GraphQLDescription("Represents a member picker.")]
public class MemberPicker : PropertyValue
{
    private readonly IEnumerable<IPublishedContent> _publishedMembers;

    /// <summary>
    /// Gets the member items of a picker
    /// </summary>
    [GraphQLDescription("Gets the member items of a picker.")]
    public List<MemberPickerItem> Members => _publishedMembers.Select(publishedMember =>
    {
        return new MemberPickerItem(publishedMember, ResolverContext);
    }).ToList();

    public MemberPicker(CreateCommand command) : base(command)
    {
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            _publishedMembers = new List<IPublishedContent> { publishedContent };
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            _publishedMembers = publishedContentItems;
        }
        else
        {
            _publishedMembers = new List<IPublishedContent>();
        }
    }
}

/// <summary>
/// Represents a member item
/// </summary>
[GraphQLDescription("Represents a member item.")]
public class MemberPickerItem
{
    private readonly IPublishedContent _publishedContent;
    private readonly string? _culture;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IResolverContext _resolverContext;

    /// <summary>
    /// Gets the name of a member item
    /// </summary>
    [GraphQLDescription("Gets the name of a member item.")]
    public string? Name => _publishedContent?.Name(_variationContextAccessor, _culture);

    /// <summary>
    /// Gets the id of a member item
    /// </summary>
    [GraphQLDescription("Gets the id of a member item.")]
    public int Id => _publishedContent.Id;

    /// <summary>
    /// Gets the key of a member item
    /// </summary>
    [GraphQLDescription("Gets the key of a member item.")]
    public Guid Key => _publishedContent.Key;

    /// <summary>
    /// Gets the properties of the member item
    /// </summary>
    [GraphQLDescription("Gets the properties of the member item.")]
    public TypedProperties Properties()
    {
        _resolverContext.SetScopedState(ContextDataKeys.PublishedContent, _publishedContent);
        return new TypedProperties();
    }

    public MemberPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        _publishedContent = publishedContent;
        _resolverContext = resolverContext;
        _culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        _variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
    }
}
