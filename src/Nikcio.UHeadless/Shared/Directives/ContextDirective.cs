namespace Nikcio.UHeadless.Shared.Directives;

public class ContextDirective : DirectiveType
{
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name("inContext");
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument("culture").Type<StringType>();
        descriptor.Argument("includePreview").Type<NonNullType<BooleanType>>();

        descriptor.Use((next, directive) => context =>
        {
            context.SetScopedState(ContextDataKeys.Culture, directive.GetArgumentValue<string?>("culture"));
            context.SetScopedState(ContextDataKeys.IsPreview, directive.GetArgumentValue<bool>("includePreview"));
            return next.Invoke(context);
        });
    }
}
