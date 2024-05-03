using Nikcio.UHeadless.Reflection;

namespace Nikcio.UHeadless.Media;

public partial class MediaItemBase
{
    public static TMediaItem? CreateMediaItem<TMediaItem>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TMediaItem : MediaItemBase
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
}
