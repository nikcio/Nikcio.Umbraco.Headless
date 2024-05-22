using Nikcio.UHeadless.Reflection;

namespace Nikcio.UHeadless.ContentItems;

public partial class ContentItemBase
{
    public static TContentItem? CreateContentItem<TContentItem>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TContentItem : ContentItemBase
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TContentItem? createdContent = dependencyReflectorFactory.GetReflectedType<TContentItem>(typeof(TContentItem), [command]);

        if (createdContent == null)
        {
            return default;
        }
        else
        {
            return createdContent;
        }
    }
}
