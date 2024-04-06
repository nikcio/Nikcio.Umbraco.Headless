using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.GraphQL.Queries;
using Nikcio.UHeadless.Extensions.Options;

namespace Nikcio.UHeadless.Extensions;

/// <summary>
/// The UHeadless extensions for GraphQL functionallity
/// </summary>
public static class UHeadlessGraphQLExtensions
{
    /// <summary>
    /// Adds Apollo tracing if the tracingOptions is set
    /// </summary>
    /// <param name="requestExecutorBuilder"></param>
    /// <param name="tracingOptions">Options for the Apollo tracing</param>
    /// <returns></returns>
    public static IRequestExecutorBuilder AddTracing(this IRequestExecutorBuilder requestExecutorBuilder, TracingOptions tracingOptions)
    {
        ArgumentNullException.ThrowIfNull(tracingOptions, nameof(tracingOptions));

        if (tracingOptions.TracingPreference != null)
        {
            requestExecutorBuilder
                .AddApolloTracing(tracingOptions.TracingPreference.GetValueOrDefault(), tracingOptions.TimestampProvider);
        }
        return requestExecutorBuilder;
    }

    /// <summary>
    /// Adds UHeadless types and GraphQL server settings
    /// </summary>
    /// <param name="requestExecutorBuilder"></param>
    /// <param name="uHeadlessGraphQLOptions"></param>
    /// <returns></returns>
    public static IRequestExecutorBuilder AddUHeadlessGraphQL(this IRequestExecutorBuilder requestExecutorBuilder, UHeadlessGraphQLOptions uHeadlessGraphQLOptions)
    {
        ArgumentNullException.ThrowIfNull(uHeadlessGraphQLOptions, nameof(uHeadlessGraphQLOptions));

        requestExecutorBuilder
            .InitializeOnStartup()
            .AddFiltering()
            .AddSorting()
            .AddQueryType<Query>()
            .AddInterfaceType<PropertyValue>();

        foreach (Type type in uHeadlessGraphQLOptions.PropertyValueTypes)
        {
            requestExecutorBuilder.AddType(type);
        }

        uHeadlessGraphQLOptions.GraphQLExtensions?.Invoke(requestExecutorBuilder);

        return requestExecutorBuilder;
    }
}
