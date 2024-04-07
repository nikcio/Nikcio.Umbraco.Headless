using Nikcio.UHeadless.Shared.Reflection;

namespace Nikcio.UHeadless.ContentItems;

public partial class ContentItemBase
{
    public static TContentItem? CreateContentItem<TContentItem>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TContentItem : class
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TContentItem? createdContent = dependencyReflectorFactory.GetReflectedType<TContentItem>(typeof(TContentItem), new object[] { command });

        if (createdContent == null)
        {
            return default;
        }
        else
        {
            return createdContent;
        }
    }

    public static IEnumerable<TContentItem?> CreateContentItems<TContentItem>(IEnumerable<CreateCommand> commands, IDependencyReflectorFactory dependencyReflectorFactory)
        where TContentItem : class
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        return commands.Select(command => CreateContentItem<TContentItem>(command, dependencyReflectorFactory));
    }
}
