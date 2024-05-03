using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Members;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.MemberItems;

/// <summary>
/// A repository to get member from Umbraco
/// </summary>
public interface IMemberItemRepository<out TMember>
    where TMember : MemberItemBase
{
    /// <summary>
    /// Gets the member model based on the member item from Umbraco
    /// </summary>
    /// <param name="command">The create command for a member item.</param>
    /// <returns></returns>
    TMember? GetMemberItem(MemberItemBase.CreateCommand command);

    /// <summary>
    /// Gets the member cache.
    /// </summary>
    /// <returns></returns>
    IPublishedMemberCache? GetCache();
}

internal class MemberItemRepository<TMember> : IMemberItemRepository<TMember>
    where TMember : MemberItemBase
{
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    private readonly ILogger<MemberItemRepository<TMember>> _logger;

    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public MemberItemRepository(IPublishedSnapshotAccessor publishedSnapshotAccessor, ILogger<MemberItemRepository<TMember>> logger, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
        _logger = logger;
        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    public TMember? GetMemberItem(MemberItemBase.CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return MemberItemBase.CreateMember<TMember>(command, _dependencyReflectorFactory);
    }

    public IPublishedMemberCache? GetCache()
    {
        if (!_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot) || publishedSnapshot == null)
        {
            _logger.LogError("Unable to get publishedSnapShot");
            return default;
        }

        return publishedSnapshot.Members;
    }
}
