using HotChocolate.Resolvers;
using static Nikcio.UHeadless.Common.Directives.DirectiveUtils;

namespace Nikcio.UHeadless.Common.Directives;

internal class ContextDirective : DirectiveType
{
    public const string DirectiveName = "inContext";

    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name(DirectiveName);
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument(ArgumentName(nameof(InvokeArguments.Culture))).Type<StringType>();
        descriptor.Argument(ArgumentName(nameof(InvokeArguments.IncludePreview))).Type<BooleanType>();
    }

    public static void Invoke(IResolverContext context, InvokeArguments arguments)
    {
        context.SetScopedState(ContextDataKeys.Culture, arguments.Culture);
        context.SetScopedState(ContextDataKeys.IncludePreview, arguments.IncludePreview ?? false);
    }

    public sealed class InvokeArguments
    {
        public required string? Culture { get; init; }

        public required bool? IncludePreview { get; init; }
    }
}
