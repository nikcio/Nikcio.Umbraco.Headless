using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Base.Composers;
using Nikcio.UHeadless.Media.Extensions;
using Nikcio.UHeadless.Media.NotificationHandlers;
using Nikcio.UHeadless.Media.TypeModules;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Media.Composers;

/// <summary>
/// Adds media services
/// </summary>
public class MediaComposer : IUHeadlessComposer
{
    /// <inheritdoc/>
    public void Compose(IUmbracoBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        if (!MediaExtensions.UsingMediaQueries)
        {
            return;
        }

        builder.Services.AddMediaServices();

        builder.AddNotificationAsyncHandler<MediaTypeChangedNotification, MediaTypeModuleMediaTypeChangedHandler>();
        builder.Services.AddSingleton<MediaTypeModule>();
    }
}
