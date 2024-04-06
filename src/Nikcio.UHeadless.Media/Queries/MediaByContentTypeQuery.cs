using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Nikcio.UHeadless.Media.Models;
using Nikcio.UHeadless.Media.Repositories;

namespace Nikcio.UHeadless.Media.Queries;

/// <summary>
/// Implements the <see cref="MediaByContentType" /> query
/// </summary>
/// <typeparam name="TMedia"></typeparam>
public class MediaByContentTypeQuery<TMedia>
    where TMedia : IMedia
{
    /// <summary>
    /// Gets all the media items by content type (Missing preview)
    /// </summary>
    /// <param name="mediaRepository"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets all the media items by content type.")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMedia?> MediaByContentType([Service] IMediaRepository<TMedia> mediaRepository,
                                                           [GraphQLDescription("The contentType to fetch.")] string contentType)
    {
        ArgumentNullException.ThrowIfNull(mediaRepository);

        return mediaRepository.GetMediaList(x =>
        {
            Umbraco.Cms.Core.Models.PublishedContent.IPublishedContentType? publishedContentType = x?.GetContentType(contentType);
            return publishedContentType != null ? x?.GetByContentType(publishedContentType) : default;
        });
    }
}
