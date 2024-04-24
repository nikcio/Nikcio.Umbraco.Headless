using HotChocolate.Resolvers;
using static Nikcio.UHeadless.Common.Directives.DirectiveUtils;

namespace Nikcio.UHeadless.Common.Directives;

internal class SegmentDirective : DirectiveType
{
    public const string DirectiveName = "useSegment";

    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name(DirectiveName);
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument(ArgumentName(nameof(InvokeArguments.Segment))).Type<StringType>();
    }

    public static void Invoke(IResolverContext context, InvokeArguments arguments)
    {
        context.SetScopedState(ContextDataKeys.Segment, arguments.Segment);
    }

    public sealed class InvokeArguments
    {
        public string? Segment { get; init; }
    }
}
