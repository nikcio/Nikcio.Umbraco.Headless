using Nikcio.UHeadless.Reflection;

namespace Nikcio.UHeadless.Members;

public partial class MemberItemBase
{
    public static TMember? CreateMember<TMember>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TMember : MemberItemBase
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TMember? createdMember = dependencyReflectorFactory.GetReflectedType<TMember>(typeof(TMember), [command]);

        if (createdMember == null)
        {
            return default;
        }
        else
        {
            return createdMember;
        }
    }
}
