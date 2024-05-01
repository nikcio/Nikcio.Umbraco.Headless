using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace Nikcio.UHeadless.Common.Directives;

internal class SegmentDirective : DirectiveType
{
    public const string DirectiveName = "useSegment";

    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name(DirectiveName);
        descriptor.Location(HotChocolate.Types.DirectiveLocation.Field);

        descriptor.Argument(DirectiveArguments.Segment).Type<StringType>();
    }

    public class DirectiveMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly DirectiveNode _directive;

        public DirectiveMiddleware(FieldDelegate next, DirectiveNode directive)
        {
            _next = next;
            _directive = directive;
        }

        public ValueTask InvokeAsync(IMiddlewareContext context)
        {
            string? segment = _directive.ArgumentValue<string?>(DirectiveArguments.Segment, context.Variables);

            context.SetScopedState(ContextDataKeys.Segment, segment);

            return _next.Invoke(context);
        }
    }

    private static class DirectiveArguments
    {
        public const string Segment = "segment";
    }
}
