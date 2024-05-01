using HotChocolate.Language;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Common.Properties;

namespace Nikcio.UHeadless.Common.Directives;

internal class FallbackDirective : DirectiveType
{
    public const string DirectiveName = "hasFallback";

    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name(DirectiveName);
        descriptor.Location(HotChocolate.Types.DirectiveLocation.Field);

        descriptor.Argument(DirectiveArguments.Fallbacks).Type<NonNullType<ListType<EnumType<PropertyFallback>>>>();
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
            List<PropertyFallback>? fallbacks = _directive.ArgumentValue<List<PropertyFallback>?>(DirectiveArguments.Fallbacks, context.Variables);

            context.SetScopedState(ContextDataKeys.Fallback, fallbacks?.ToFallback() ?? default);

            return _next.Invoke(context);
        }
    }

    private static class DirectiveArguments
    {
        public const string Fallbacks = "fallbacks";
    }
}
