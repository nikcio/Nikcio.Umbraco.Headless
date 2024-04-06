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
/// Implements the <see cref="ContentByContentType" /> query
/// </summary>
/// <typeparam name="TContent"></typeparam>
public class ContentByContentTypeQuery<TContent>
    where TContent : IContent
{
    /// <summary>
    /// Gets all the content items by content type
    /// </summary>
    /// <param name="variationContextAccessor"></param>
    /// <param name="preview"></param>
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
                                                           [Service] IVariationContextAccessor variationContextAccessor,
                                                           [GraphQLDescription("The contentType to fetch.")] string contentType,
                                                           [GraphQLDescription("The culture.")] string? culture = null,
                                                           [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
                                                           [GraphQLDescription("The property variation segment")] string? segment = null,
                                                           [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentRepository);

        return contentRepository.GetContentList(x =>
        {
            IPublishedContentType? publishedContentType = x?.GetContentType(contentType);

            if(publishedContentType == null)
            {
                return default;
            }

            return x?.GetAtRoot(preview, culture)
                    .SelectMany(content => content.DescendantsOrSelf(variationContextAccessor, culture))
                    .Where(content => content.ContentType.Id == publishedContentType.Id);
        }, culture, segment, fallback?.ToFallback());
    }
}
