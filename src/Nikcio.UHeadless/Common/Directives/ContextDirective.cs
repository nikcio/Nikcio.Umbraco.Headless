using HotChocolate.Resolvers;
using HotChocolate.Types;
using static System.Net.Mime.MediaTypeNames;

namespace Nikcio.UHeadless.Common.Directives;

public class ContextDirective : DirectiveType
{
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name("inContext");
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument("culture").Type<StringType>();
        descriptor.Argument("includePreview").Type<NonNullType<BooleanType>>();

        descriptor.Use<DirectiveMiddleware>();

        //descriptor.Use((next, directive) => context =>
        //{
        //    context.SetScopedState(ContextDataKeys.Culture, directive.GetArgumentValue<string?>("culture"));
        //    context.SetScopedState(ContextDataKeys.IncludePreview, directive.GetArgumentValue<bool>("includePreview"));
        //    return next.Invoke(context);
        //});
    }

    public class DirectiveMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly Directive _directive;

        public DirectiveMiddleware(FieldDelegate next, Directive directive)
        {
            _next = next;
            _directive = directive;
        }

        public ValueTask InvokeAsync(IMiddlewareContext context)
        {
            context.SetScopedState(ContextDataKeys.Culture, _directive.GetArgumentValue<string?>("culture"));
            context.SetScopedState(ContextDataKeys.IncludePreview, _directive.GetArgumentValue<bool>("includePreview"));
            return _next.Invoke(context);
        }
    }
}
