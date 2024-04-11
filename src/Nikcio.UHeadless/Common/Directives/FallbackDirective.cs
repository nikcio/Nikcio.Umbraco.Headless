using Nikcio.UHeadless.Common.Properties;

namespace Nikcio.UHeadless.Common.Directives;

public class FallbackDirective : DirectiveType
{
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name("hasFallback");
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument("fallbacks").Type<NonNullType<ListType<EnumType<PropertyFallback>>>>();

        descriptor.Use((next, directive) => context =>
        {
            context.SetScopedState(ContextDataKeys.Fallback, directive.GetArgumentValue<List<PropertyFallback>>("fallbacks")?.ToFallback());
            return next.Invoke(context);
        });
    }
}
