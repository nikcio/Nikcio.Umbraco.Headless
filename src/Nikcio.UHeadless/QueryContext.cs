using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Nikcio.UHeadless.Properties;

namespace Nikcio.UHeadless;

/// <summary>
/// Represents the context of a query
/// </summary>
/// <remarks>
/// Remember to initialize the QueryContext with <see cref="Initialize(IResolverContext)"/>
/// </remarks>
[GraphQLDescription("Represents the context of a query.")]
public class QueryContext
{
    /// <summary>
    /// The culture of the query
    /// </summary>
    [GraphQLDescription("The culture of the query.")]
    public string? Culture { get; set; }

    /// <summary>
    /// Whether to include preview content
    /// </summary>
    [GraphQLDescription("Whether to include preview content.")]
    public bool? IncludePreview { get; set; }

    /// <summary>
    /// The fallbacks to use on a property value
    /// </summary>
    /// <remarks>
    /// To get the Umbraco native Fallback struct use <c>.ToFallback()</c>
    /// </remarks>
    [GraphQLDescription("The fallbacks to use on a property value.")]
    public List<PropertyFallback>? Fallbacks { get; set; }

    /// <summary>
    /// The segment to use on a property value
    /// </summary>
    [GraphQLDescription("The segment to use on a property value.")]
    public string? Segment { get; set; }

    /// <summary>
    /// The base URL of the request.
    /// </summary>
    [GraphQLDescription("The base URL of the request. Example: https://my-website.com. Used to return the correct URLs on content items")]
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Initializes the context values with the correct context keys keys
    /// </summary>
    [MemberNotNullWhen(true, nameof(IncludePreview), nameof(Fallbacks), nameof(BaseUrl))]
    public bool Initialize(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IncludePreview ??= false;
        Fallbacks ??= [];

        HttpContext? httpContext = resolverContext.Service<IHttpContextAccessor>().HttpContext;

        if (httpContext == null)
        {
            return false;
        }

        BaseUrl ??= $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Host}";

        resolverContext.Initialize(this);

        return true;
    }
}
