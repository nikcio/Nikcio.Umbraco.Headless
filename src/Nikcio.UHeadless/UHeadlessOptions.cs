using HotChocolate.Execution.Configuration;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core.DependencyInjection;

namespace Nikcio.UHeadless;

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
    /// [WARNING] Disables authorization for the GraphQL server.
    /// This must be set before <see cref="UHeadlessExtensions.AddDefaults(UHeadlessOptions)"/>
    /// </summary>
    /// <remarks>
    /// Only use this if you secure your GraphQL server in another way.
    /// This will expose all your Umbraco data without any authorization.
    /// </remarks>
    public bool DisableAuthorization { get; set; }

    /// <summary>
    /// The HotChocolate request executor builder
    /// </summary>
    /// <remarks>
    /// Used to customize the GraphQL server.
    /// </remarks>
    public required IRequestExecutorBuilder RequestExecutorBuilder { get; init; }

    /// <summary>
    /// The Umbraco builder
    /// </summary>
    public required IUmbracoBuilder UmbracoBuilder { get; init; }
}
