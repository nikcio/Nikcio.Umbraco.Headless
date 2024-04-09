using HotChocolate;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Shared;
using Nikcio.UHeadless.Shared.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.Content.Queries.ContentByRoute;

public partial class ContentItem : ContentItemBase
{
    private readonly IVariationContextAccessor _variationContextAccessor;

    public ContentItem(CreateCommand command, IVariationContextAccessor variationContextAccessor) : base(command)
    {
        _variationContextAccessor = variationContextAccessor;
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
    public string? Url(UrlMode urlMode) => PublishedContent?.Url(Culture, urlMode);

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
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
