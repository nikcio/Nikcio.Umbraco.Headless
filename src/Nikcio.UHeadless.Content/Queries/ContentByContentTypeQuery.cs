using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Nikcio.UHeadless.Base.Properties.Extensions;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.Repositories;

namespace Nikcio.UHeadless.Content.Queries;

/// <summary>
/// Implements the <see cref="ContentByContentType" /> query
/// </summary>
/// <typeparam name="TContent"></typeparam>
public class ContentByContentTypeQuery<TContent>
    where TContent : IContent
{
    /// <summary>
    /// Gets all the content items by content type (Missing preview)
    /// </summary>
    /// <param name="contentRepository"></param>
    /// <param name="contentType"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets all the content items by content type.")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TContent?> ContentByContentType([Service] IContentRepository<TContent> contentRepository,
                                                           [GraphQLDescription("The contentType to fetch.")] string contentType,
                                                           [GraphQLDescription("The culture.")] string? culture = null,
                                                           [GraphQLDescription("The property variation segment")] string? segment = null,
                                                           [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentRepository, nameof(contentRepository));

        return contentRepository.GetContentList(x =>
        {
            Umbraco.Cms.Core.Models.PublishedContent.IPublishedContentType? publishedContentType = x?.GetContentType(contentType);
            return publishedContentType != null ? x?.GetByContentType(publishedContentType) : default;
        }, culture, segment, fallback?.ToFallback());
    }
}