using System.ComponentModel.DataAnnotations;
using HotChocolate.Execution.Configuration;
using Nikcio.UHeadless.Defaults.Auth;
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

    /// <summary>
    /// The authorization options
    /// This must be set if <see cref="DisableAuthorization"/> is false.
    /// </summary>
    internal UHeadlessAuthorizationOptions? AuthorizationOptions { get; set; }

    /// <summary>
    /// Indicates if the options has queries added.
    /// </summary>
    internal bool HasQueries { get; set; }

    /// <summary>
    /// Indicates if the options has mutations added.
    /// </summary>
    internal bool HasMutations { get; set; }
}

/// <summary>
/// Options for UHeadless authorization
/// </summary>
public class UHeadlessAuthorizationOptions
{
    /// <summary>
    /// The issuer of the JWT tokens
    /// </summary>
    public string Issuer { get; set; } = "Nikcio.UHeadless";

    /// <summary>
    /// The audience of the JWT tokens
    /// </summary>
    public string Audience { get; set; } = "Nikcio.UHeadless";

    /// <summary>
    /// The secret used for signing the JWT tokens
    /// </summary>
    /// <remarks>
    /// Must be at least 64 characters long.
    /// </remarks>
    [MinLength(64)]
    public required string Secret { get; set; }

    /// <summary>
    /// The expiry minutes for the JWT tokens
    /// </summary>
    public int ExpiryMinutes { get; set; } = 30;

    /// <summary>
    /// The API key used for creating JWT tokens.
    /// </summary>
    /// <remarks>
    /// Must be at least 32 characters long.
    /// </remarks>
    [MinLength(32)]
    public required string ApiKey { get; set; }

    /// <summary>
    /// Indicates if authorization is disabled.
    /// </summary>
    internal bool IsAuthorizationDisabled { get; init; }
}
