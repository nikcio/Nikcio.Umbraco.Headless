using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.ContentItems.NotificationHandlers;

/// <summary>
/// Triggers GraphQL schema rebuild when content types changes
/// </summary>
internal class ContentTypeChangedHandler : INotificationAsyncHandler<ContentTypeChangedNotification>
{
    private readonly ContentTypeModule _contentTypeModule;

    public ContentTypeChangedHandler(ContentTypeModule contentTypeModule)
    {
        _contentTypeModule = contentTypeModule;
    }

    public Task HandleAsync(ContentTypeChangedNotification notification, CancellationToken cancellationToken)
    {
        _contentTypeModule.OnTypesChanged(EventArgs.Empty);

        return Task.CompletedTask;
    }
}
