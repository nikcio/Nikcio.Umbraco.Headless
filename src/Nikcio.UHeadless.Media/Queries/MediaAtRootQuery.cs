using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Nikcio.UHeadless.Media.Models;
using Nikcio.UHeadless.Media.Repositories;

namespace Nikcio.UHeadless.Media.Queries;

/// <summary>
/// Implements the <see cref="MediaAtRoot" /> query
/// </summary>
/// <typeparam name="TMedia"></typeparam>
public class MediaAtRootQuery<TMedia>
    where TMedia : IMedia
{
    /// <summary>
    /// Gets all the Media items at root level
    /// </summary>
    /// <param name="MediaRepository"></param>
    /// <param name="preview"></param>
    /// <returns></returns>
    [GraphQLDescription("Gets all the Media items at root level.")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public virtual IEnumerable<TMedia?> MediaAtRoot([Service] IMediaRepository<TMedia> MediaRepository,
                                                    [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false)
    {
        ArgumentNullException.ThrowIfNull(MediaRepository);

        return MediaRepository.GetMediaList(x => x?.GetAtRoot(preview));
    }
}
