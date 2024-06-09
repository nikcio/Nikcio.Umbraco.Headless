using HotChocolate.Resolvers;
using Nikcio.UHeadless;
using Nikcio.UHeadless.Common.Properties;

namespace Code.Examples.Headless.CustomEditorExample;

public class CustomEditor : PropertyValue
{
    public CustomEditor(CreateCommand command) : base(command)
    {
        ArgumentNullException.ThrowIfNull(command);

        IResolverContext resolverContext = command.ResolverContext;
        Value = PublishedProperty.Value<string>(PublishedValueFallback, resolverContext.Culture(), resolverContext.Segment(), resolverContext.Fallback());
    }

    public string? Value { get; }
}
