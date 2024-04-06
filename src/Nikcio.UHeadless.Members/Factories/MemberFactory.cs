using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Base.Elements.Commands;
using Nikcio.UHeadless.Core.Reflection.Factories;
using Nikcio.UHeadless.Members.Commands;
using Nikcio.UHeadless.Members.Models;
using Umbraco.Cms.Core.PublishedCache;

namespace Nikcio.UHeadless.Members.Factories;

/// <inheritdoc/>
public class MemberFactory<TMember> : IMemberFactory<TMember>
    where TMember : IMember
{
    /// <summary>
    /// A factory for creating object with DI
    /// </summary>
    protected IDependencyReflectorFactory dependencyReflectorFactory { get; }

    /// <summary>
    /// A published snapshot
    /// </summary>
    protected IPublishedSnapshotAccessor publishedSnapshotAccessor { get; }

    /// <summary>
    /// The logger
    /// </summary>
    protected ILogger<MemberFactory<TMember>> logger { get; }

    /// <inheritdoc/>
    public MemberFactory(IDependencyReflectorFactory dependencyReflectorFactory, IPublishedSnapshotAccessor publishedSnapshotAccessor, ILogger<MemberFactory<TMember>> logger)
    {
        this.dependencyReflectorFactory = dependencyReflectorFactory;
        this.publishedSnapshotAccessor = publishedSnapshotAccessor;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public virtual TMember? CreateMember(Umbraco.Cms.Core.Models.IMember member)
    {
        if (publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot))
        {
            if (publishedSnapshot is null)
            {
                logger.LogError("Unable to get publishedSnapShot");
                return default;
            }
            Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent? publishedMember = publishedSnapshot.Members?.Get(member);

            var createElementCommand = new CreateElement(publishedMember, null, null, null);
            var createMemberCommand = new CreateMember(publishedMember, createElementCommand);

            IMember? createdContent = dependencyReflectorFactory.GetReflectedType<IMember>(typeof(TMember), new object[] { createMemberCommand });
            return createdContent == null ? default : (TMember) createdContent;
        }
        logger.LogError("Unable to get publishedSnapShot");
        return default;
    }
}
