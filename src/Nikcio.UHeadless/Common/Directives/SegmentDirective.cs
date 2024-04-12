namespace Nikcio.UHeadless.Common.Directives;

public class SegmentDirective : DirectiveType
{
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        descriptor.Name("useSegment");
        descriptor.Location(DirectiveLocation.Field);

        descriptor.Argument("segment").Type<StringType>();

        descriptor.Use((next, directive) => context =>
        {
            context.SetScopedState(ContextDataKeys.Segment, directive.GetArgumentValue<string?>("segment"));
            return next.Invoke(context);
        });
    }
}
