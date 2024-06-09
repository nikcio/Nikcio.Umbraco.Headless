using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.TypeModules;

/// <summary>
/// Triggers GraphQL schema rebuild when Umbraco started
/// </summary>
internal class UmbracoStartedHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly UmbracoTypeModule _contentTypeModule;

    public UmbracoStartedHandler(UmbracoTypeModule contentTypeModule)
    {
        _contentTypeModule = contentTypeModule;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        if (!_contentTypeModule.IsInitialized)
        {
            _contentTypeModule.OnTypesChanged(EventArgs.Empty);
        }

        return Task.CompletedTask;
    }
}
