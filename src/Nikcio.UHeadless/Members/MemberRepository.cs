using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Common.Reflection;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Members;

/// <summary>
/// A repository to get member from Umbraco
/// </summary>
public interface IMemberRepository<TMember>
    where TMember : MemberBase
{
    /// <summary>
    /// Gets the member model based on the member item from Umbraco
    /// </summary>
    /// <param name="command">The create command for a member item.</param>
    /// <returns></returns>
    TMember? GetMemberItem(MemberBase.CreateCommand command);

    /// <summary>
    /// Gets the member cache.
    /// </summary>
    /// <returns></returns>
    IPublishedMemberCache? GetCache();
}

internal class MemberRepository<TMember> : IMemberRepository<TMember>
    where TMember : MemberBase
{
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    private readonly ILogger<MemberRepository<TMember>> _logger;

    private readonly IDependencyReflectorFactory _dependencyReflectorFactory;

    public MemberRepository(IPublishedSnapshotAccessor publishedSnapshotAccessor, ILogger<MemberRepository<TMember>> logger, IDependencyReflectorFactory dependencyReflectorFactory)
    {
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
        _logger = logger;
        _dependencyReflectorFactory = dependencyReflectorFactory;
    }

    public TMember? GetMemberItem(MemberBase.CreateCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        return MemberBase.CreateMember<TMember>(command, _dependencyReflectorFactory);
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
