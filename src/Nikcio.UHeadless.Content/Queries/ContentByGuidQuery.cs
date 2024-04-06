using HotChocolate;
using HotChocolate.Data;
using Nikcio.UHeadless.Base.Properties.Extensions;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.Repositories;

namespace Nikcio.UHeadless.Content.Queries;

/// <summary>
/// Implements the <see cref="ContentByGuid" /> query
/// </summary>
/// <typeparam name="TContent"></typeparam>
public class ContentByGuidQuery<TContent>
    where TContent : IContent
{
    /// <summary>
    /// Gets a content item by guid
    /// </summary>
    /// <param name="contentRepository"></param>
    /// <param name="id"></param>
    /// <param name="culture"></param>
    /// <param name="preview"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets a content item by guid.")]
    [UseFiltering]
    [UseSorting]
    public virtual TContent? ContentByGuid([Service] IContentRepository<TContent> contentRepository,
                                                [GraphQLDescription("The id to fetch.")] Guid id,
                                                [GraphQLDescription("The culture to fetch.")] string? culture = null,
                                                [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
                                                [GraphQLDescription("The property variation segment")] string? segment = null,
                                                [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentRepository);

        return contentRepository.GetContent(x => x?.GetById(preview, id), culture, segment, fallback?.ToFallback());
    }
}