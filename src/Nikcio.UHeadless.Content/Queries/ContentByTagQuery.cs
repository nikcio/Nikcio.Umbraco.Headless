using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Nikcio.UHeadless.Base.Properties.Extensions;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Models;
using Nikcio.UHeadless.Content.Repositories;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Content.Queries;

/// <summary>
/// Implements the <see cref="ContentByTag" /> query
/// </summary>
/// <typeparam name="TContent"></typeparam>
public class ContentByTagQuery<TContent>
    where TContent : IContent
{
    /// <summary>
    /// Gets content items by tag
    /// </summary>
    /// <param name="contentRepository"></param>
    /// <param name="tagService"></param>
    /// <param name="tag"></param>
    /// <param name="tagGroup"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets content items by tag.")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TContent?> ContentByTag([Service] IContentRepository<TContent> contentRepository,
                                                [Service] ITagService tagService,
                                                [GraphQLDescription("The tag to fetch.")] string tag,
                                                [GraphQLDescription("The tag group to fetch.")] string? tagGroup = null,
                                                [GraphQLDescription("The culture to fetch.")] string? culture = null,
                                                [GraphQLDescription("The property variation segment")] string? segment = null,
                                                [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        ArgumentNullException.ThrowIfNull(contentRepository);

        return contentRepository.GetContentList(x =>
        {
            IEnumerable<Umbraco.Cms.Core.Models.TaggedEntity> taggedEntities = tagService.GetTaggedContentByTag(tag, tagGroup, culture);
            return taggedEntities.Select(entity => x?.GetById(entity.EntityId)).OfType<IPublishedContent>();
        }, culture, segment, fallback?.ToFallback());
    }
}
