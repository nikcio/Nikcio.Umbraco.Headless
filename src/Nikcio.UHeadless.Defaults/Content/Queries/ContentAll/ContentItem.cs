using HotChocolate;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Shared;
using Nikcio.UHeadless.Shared.Properties;
using Nikcio.UHeadless.Shared.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Content.Queries.ContentAll;

[GraphQLName("ContentAllContentItem")]
public class ContentItem : ContentItemBase
{
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public ContentItem(CreateCommand command, IVariationContextAccessor variationContextAccessor, IDependencyReflectorFactory dependencyReflectorFactory) : base(command)
    {
        ArgumentNullException.ThrowIfNull(variationContextAccessor);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);
        ArgumentNullException.ThrowIfNull(command);

        _variationContextAccessor = variationContextAccessor;
        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    /// <summary>
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public string? UrlSegment => PublishedContent?.UrlSegment(_variationContextAccessor, Culture);

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    public string? Url(UrlMode urlMode)
    {
        return PublishedContent?.Url(Culture, urlMode);
    }

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public string? Name => PublishedContent?.Name(_variationContextAccessor, Culture);

    /// <summary>
    /// Gets the id of a content item
    /// </summary>
    [GraphQLDescription("Gets the id of a content item.")]
    public int? Id => PublishedContent?.Id;

    /// <summary>
    /// Gets the key of a content item
    /// </summary>
    [GraphQLDescription("Gets the key of a content item.")]
    public Guid? Key => PublishedContent?.Key;

    /// <summary>
    /// Gets the identifier of the template to use to render the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the template to use to render the content item.")]
    public int? TemplateId => PublishedContent?.TemplateId;

    /// <summary>
    /// Gets the date the content item was last updated
    /// </summary>
    [GraphQLDescription("Gets the date the content item was last updated.")]
    public DateTime? UpdateDate => PublishedContent?.UpdateDate;

    /// <summary>
    /// Gets the parent of the content item
    /// </summary>
    [GraphQLDescription("Gets the parent of the content item.")]
    public ContentItem? Parent => PublishedContent?.Parent != null ? CreateContentItem<ContentItem>(new CreateCommand()
    {
        Culture = Culture,
        Fallback = Fallback,
        IsPreview = IsPreview,
        PublishedContent = PublishedContent.Parent,
        ResolverContext = ResolverContext,
        Segment = Segment,
    }, _dependencyReflectorFactory) : default;

    /// <summary>
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
