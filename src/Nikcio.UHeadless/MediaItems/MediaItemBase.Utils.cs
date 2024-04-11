using Nikcio.UHeadless.Common.Reflection;

namespace Nikcio.UHeadless.Media;

public partial class MemberItem
{
    public static TMediaItem? CreateMediaItem<TMediaItem>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TMediaItem : MemberItem
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TMediaItem? createdMediaItem = dependencyReflectorFactory.GetReflectedType<TMediaItem>(typeof(TMediaItem), new object[] { command });

        if (createdMediaItem == null)
        {
            return default;
        }
        else
        {
            return createdMediaItem;
        }
    }

    public static IEnumerable<TMediaItem?> CreateMediaItems<TMediaItem>(IEnumerable<CreateCommand> commands, IDependencyReflectorFactory dependencyReflectorFactory)
        where TMediaItem : MemberItem
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        return commands.Select(command => CreateMediaItem<TMediaItem>(command, dependencyReflectorFactory));
    }
}
