using HotChocolate.Execution;
using HotChocolate.Language;

namespace Nikcio.UHeadless.Common.Directives;

internal static class DirectiveUtils
{
    internal static T? ArgumentValue<T>(this DirectiveNode directive, string argumentName, IVariableValueCollection variables)
    {
        ArgumentNode? argument = directive.Arguments.FirstOrDefault(x => x.Name.Value == argumentName);

        if (argument?.Value.Kind == SyntaxKind.Variable)
        {
            return (T?) (variables.FirstOrDefault(x => x.Name == ((VariableNode) argument.Value).Name.Value).Value?.Value ?? default(T));
        }

        return (T?) (argument?.Value?.Value ?? default(T));
    }
}
