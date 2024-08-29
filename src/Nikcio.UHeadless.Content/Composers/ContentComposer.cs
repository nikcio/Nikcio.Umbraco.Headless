using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Base.Composers;
using Nikcio.UHeadless.Content.Extensions;
using Nikcio.UHeadless.Content.NotificationHandlers;
using Nikcio.UHeadless.Content.TypeModules;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Content.Composers;

/// <summary>
/// Adds content services
/// </summary>
public class ContentComposer : IUHeadlessComposer
{
    /// <inheritdoc/>
    public void Compose(IUmbracoBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!ContentExtensions.UsingContentQueries)
        {
            return;
        }

        builder.Services.AddContentServices();

        builder.AddNotificationAsyncHandler<ContentTypeChangedNotification, ContentTypeModuleContentTypeChangedHandler>();
        builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, UmbracoStartedHandler>();
        builder.Services.AddSingleton<ContentTypeModule>();
    }
}
