using Nikcio.UHeadless.Common.TypeModules;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.MediaItems;

/// <summary>
/// Triggers GraphQL schema rebuild when media types changes
/// </summary>
internal class MediaTypeChangedHandler : INotificationAsyncHandler<MediaTypeChangedNotification>
{
    private readonly UmbracoTypeModule _mediaTypeModule;

    public MediaTypeChangedHandler(UmbracoTypeModule mediaTypeModule)
    {
        _mediaTypeModule = mediaTypeModule;
    }

    public Task HandleAsync(MediaTypeChangedNotification notification, CancellationToken cancellationToken)
    {
        _mediaTypeModule.OnTypesChanged(EventArgs.Empty);

        return Task.CompletedTask;
    }
}
