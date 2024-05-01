using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace Nikcio.UHeadless.Common.Directives;

internal class ContextDirective : DirectiveType
{
    public const string DirectiveName = "inContext";

    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name(DirectiveName);
        descriptor.Location(HotChocolate.Types.DirectiveLocation.Field);

        descriptor.Argument(DirectiveArguments.Culture).Type<StringType>();
        descriptor.Argument(DirectiveArguments.IncludePreview).Type<BooleanType>();

        descriptor.Use<DirectiveMiddleware>();
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
            string? culture = _directive.ArgumentValue<string?>(DirectiveArguments.Culture, context.Variables);
            bool? includePreview = _directive.ArgumentValue<bool?>(DirectiveArguments.IncludePreview, context.Variables);

            context.SetScopedState(ContextDataKeys.Culture, culture);
            context.SetScopedState(ContextDataKeys.IncludePreview, includePreview ?? false);

            return _next.Invoke(context);
        }
    }

    private static class DirectiveArguments
    {
        public const string Culture = "culture";

        public const string IncludePreview = "includePreview";
    }
}
