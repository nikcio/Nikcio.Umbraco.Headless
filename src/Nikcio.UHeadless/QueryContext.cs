using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
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
    /// Initializes the context values with the correct context keys keys
    /// </summary>
    [MemberNotNullWhen(true, nameof(IncludePreview), nameof(Fallbacks))]
    public bool Initialize(IResolverContext resolverContext)
    {
        resolverContext.Initialize(this);

        IncludePreview ??= false;
        Fallbacks ??= new();

        return true;
    }
}
