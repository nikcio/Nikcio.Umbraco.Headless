using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Options;
using Nikcio.UHeadless.Common.Properties;

namespace Nikcio.UHeadless.Extensions;

/// <summary>
/// Options for UHeadless
/// </summary>
public class UHeadlessOptions
{
    /// <summary>
    /// The property map
    /// </summary>
    /// <remarks>
    /// Used to register custom models to specific property values.
    /// </remarks>
    public IPropertyMap PropertyMap { get; } = new PropertyMap();

    /// <summary>
    /// The HotChocolate request executor builder
    /// </summary>
    /// <remarks>
    /// Used to customize the GraphQL server.
    /// </remarks>
    public required IRequestExecutorBuilder RequestExecutorBuilder { get; init; }
}
