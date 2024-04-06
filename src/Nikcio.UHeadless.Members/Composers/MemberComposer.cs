using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Base.Composers;
using Nikcio.UHeadless.Members.Extensions;
using Nikcio.UHeadless.Members.NotificationHandlers;
using Nikcio.UHeadless.Members.TypeModules;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Members.Composers;

/// <summary>
/// Adds member services
/// </summary>
public class MemberComposer : IUHeadlessComposer
{
    /// <inheritdoc/>
    public void Compose(IUmbracoBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (!MemberExtensions.UsingMemberQueries)
        {
            return;
        }

        builder.Services.AddMemberServices();

        builder.AddNotificationAsyncHandler<MemberTypeChangedNotification, MemberTypeModuleMemberChangedHandler>();
        builder.Services.AddSingleton<MemberTypeModule>();
    }
}
