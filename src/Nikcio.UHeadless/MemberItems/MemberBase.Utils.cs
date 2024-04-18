using Nikcio.UHeadless.Common.Reflection;

namespace Nikcio.UHeadless.Members;

public partial class MemberItemBase
{
    public static TMember? CreateMember<TMember>(CreateCommand command, IDependencyReflectorFactory dependencyReflectorFactory)
        where TMember : MemberItemBase
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        TMember? createdMember = dependencyReflectorFactory.GetReflectedType<TMember>(typeof(TMember), new object[] { command });

        if (createdMember == null)
        {
            return default;
        }
        else
        {
            return createdMember;
        }
    }

    public static IEnumerable<TMember?> CreateMembers<TMember>(IEnumerable<CreateCommand> commands, IDependencyReflectorFactory dependencyReflectorFactory)
        where TMember : MemberItemBase
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(dependencyReflectorFactory);

        return commands.Select(command => CreateMember<TMember>(command, dependencyReflectorFactory));
    }
}
