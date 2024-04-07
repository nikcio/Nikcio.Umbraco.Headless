using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Content.NotificationHandlers;

/// <summary>
/// Triggers GraphQL schema rebuild when content types changes
/// </summary>
internal class ContentTypeChangedHandler : INotificationAsyncHandler<ContentTypeChangedNotification>
{
    private readonly ContentTypeModule _contentTypeModule;

    /// <inheritdoc/>
    public ContentTypeChangedHandler(ContentTypeModule contentTypeModule)
    {
        _contentTypeModule = contentTypeModule;
    }

    /// <inheritdoc/>
    public Task HandleAsync(ContentTypeChangedNotification notification, CancellationToken cancellationToken)
    {
        _contentTypeModule.OnTypesChanged(EventArgs.Empty);

        return Task.CompletedTask;
    }
}
