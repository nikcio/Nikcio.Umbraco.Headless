using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Nikcio.UHeadless.Base.Properties.Extensions;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.Repositories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Content.Queries;

/// <summary>
/// Implements the <see cref="ContentAll" /> query
/// </summary>
/// <typeparam name="TContent"></typeparam>
public class ContentAllQuery<TContent>
    where TContent : IContent
{
    /// <summary>
    /// Gets all the content items available
    /// </summary>
    /// <param name="contentRepository"></param>
    /// <param name="culture"></param>
    /// <param name="preview"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets all the content items available.")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TContent?> ContentAll([Service] IContentRepository<TContent> contentRepository,
                                                           [GraphQLDescription("The culture.")] string? culture = null,
                                                           [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
                                                           [GraphQLDescription("The property variation segment")] string? segment = null,
                                                           [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentRepository);

        return contentRepository.GetContentList(x =>
        {
            return x?.GetAtRoot(preview, culture).SelectMany(content => content.Descendants(culture))
                .Concat(x?.GetAtRoot(preview, culture) ?? Enumerable.Empty<IPublishedContent>());
        }, culture, segment, fallback?.ToFallback());
    }
}