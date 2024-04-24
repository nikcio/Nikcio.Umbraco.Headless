using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common.Properties;
using static Nikcio.UHeadless.Common.Directives.DirectiveUtils;

namespace Nikcio.UHeadless.Common.Directives;

internal class FallbackDirective : DirectiveType
{
    public const string DirectiveName = "hasFallback";

    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name(DirectiveName);
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument(ArgumentName(nameof(InvokeArguments.Fallbacks))).Type<NonNullType<ListType<EnumType<PropertyFallback>>>>();
    }

    public static void Invoke(IResolverContext context, InvokeArguments arguments)
    {
        context.SetScopedState(ContextDataKeys.Fallback, arguments.Fallbacks);
    }

    public sealed class InvokeArguments
    {
        public required List<PropertyFallback>? Fallbacks { get; init; }
    }
}
