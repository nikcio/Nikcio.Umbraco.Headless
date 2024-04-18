using Nikcio.UHeadless.Common.TypeModules;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.MemberItems;

/// <summary>
/// Triggers GraphQL schema rebuild when member types changes
/// </summary>
internal class MemberTypeChangedHandler : INotificationAsyncHandler<MemberTypeChangedNotification>
{
    private readonly UmbracoTypeModule _memberTypeModule;

    /// <inheritdoc/>
    public MemberTypeChangedHandler(UmbracoTypeModule memberTypeModule)
    {
        _memberTypeModule = memberTypeModule;
    }

    /// <inheritdoc/>
    public Task HandleAsync(MemberTypeChangedNotification notification, CancellationToken cancellationToken)
    {
        _memberTypeModule.OnTypesChanged(EventArgs.Empty);

        return Task.CompletedTask;
    }
}
