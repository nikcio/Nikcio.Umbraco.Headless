using Nikcio.UHeadless.Shared.Properties;

namespace Nikcio.UHeadless.Extensions.Options;

/// <summary>
/// Options for UHeadless
/// </summary>
public class UHeadlessOptions
{
    /// <summary>
    /// Options for the property services
    /// </summary>
    public virtual PropertyServicesOptions PropertyServicesOptions { get; set; } = new();

    /// <summary>
    /// Options for the Apollo tracing
    /// </summary>
    public virtual TracingOptions TracingOptions { get; set; } = new();

    /// <summary>
    /// Options for UHeadless GraphQL
    /// </summary>
    public virtual UHeadlessGraphQLOptions UHeadlessGraphQLOptions { get; set; } = new();
}

/// <summary>
/// Options for the property services
/// </summary>
public class PropertyServicesOptions
{
    /// <summary>
    /// Options for the property map
    /// </summary>
    public virtual PropertyMapOptions PropertyMapOptions { get; set; } = new();
}

/// <summary>
/// Options for the property map
/// </summary>
public class PropertyMapOptions
{
    /// <summary>
    /// Any custom mappings of properties
    /// </summary>
    public virtual List<Action<IPropertyMap>>? PropertyMappings { get; set; }

    /// <summary>
    /// The property map
    /// </summary>
    /// <remarks>
    /// This will be used to create the singleton
    /// </remarks>
    public virtual IPropertyMap PropertyMap { get; set; } = new PropertyMap();
}
