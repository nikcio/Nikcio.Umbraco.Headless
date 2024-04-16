using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Common.Properties.Models;

/// <summary>
/// Represents a member picker
/// </summary>
[GraphQLDescription("Represents a member picker.")]
public class MemberPickerResponse : PropertyValue
{
    private readonly IEnumerable<IPublishedContent> _publishedMembers;
    private readonly bool _isMultiple;

    /// <summary>
    /// Gets the member items of a picker
    /// </summary>
    [GraphQLDescription("Gets the member items of a picker.")]
    public List<MemberPickerItemResponse> Members => _publishedMembers.Select(publishedMember =>
    {
        return new MemberPickerItemResponse(publishedMember, Culture, ResolverContext);
    }).ToList();

    /// <summary>
    /// Whether the picker has multiple items
    /// </summary>
    [GraphQLDescription("Whether the picker has multiple items.")]
    public bool IsMultiple => _isMultiple;

    public MemberPickerResponse(CreateCommand command) : base(command)
    {
        object? publishedContentItemsAsObject = PublishedProperty.Value<object>(PublishedValueFallback, Culture, Segment, Fallback);

        if (publishedContentItemsAsObject is IPublishedContent publishedContent)
        {
            _publishedMembers = new List<IPublishedContent> { publishedContent };
            _isMultiple = false;
        }
        else if (publishedContentItemsAsObject is IEnumerable<IPublishedContent> publishedContentItems)
        {
            _publishedMembers = publishedContentItems;
            _isMultiple = true;
        }
        else
        {
            _publishedMembers = new List<IPublishedContent>();
            _isMultiple = false;
        }
    }
}

/// <summary>
/// Represents a member item
/// </summary>
[GraphQLDescription("Represents a member item.")]
public class MemberPickerItemResponse
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

    public MemberPickerItemResponse(IPublishedContent publishedContent, string? culture, IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        _publishedContent = publishedContent;
        _culture = culture;
        _variationContextAccessor = resolverContext.Service<IVariationContextAccessor>();
        _resolverContext = resolverContext;
    }
}
