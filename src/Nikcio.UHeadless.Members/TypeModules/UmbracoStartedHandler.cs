using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Members.TypeModules;

/// <summary>
/// Triggers GraphQL schema rebuild when Umbraco started
/// </summary>
internal class UmbracoStartedHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly MemberTypeModule _memberTypeModule;

    public UmbracoStartedHandler(MemberTypeModule memberTypeModule)
    {
        _memberTypeModule = memberTypeModule;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        if (!_memberTypeModule.IsInitialized)
        {
            _memberTypeModule.OnTypesChanged(EventArgs.Empty);
        }

        return Task.CompletedTask;
    }
}
