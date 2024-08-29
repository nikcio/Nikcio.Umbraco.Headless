using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Media.TypeModules;

/// <summary>
/// Triggers GraphQL schema rebuild when Umbraco started
/// </summary>
internal class UmbracoStartedHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly MediaTypeModule _mediaTypeModule;

    public UmbracoStartedHandler(MediaTypeModule mediaTypeModule)
    {
        _mediaTypeModule = mediaTypeModule;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        if (!_mediaTypeModule.IsInitialized)
        {
            _mediaTypeModule.OnTypesChanged(EventArgs.Empty);
        }

        return Task.CompletedTask;
    }
}
