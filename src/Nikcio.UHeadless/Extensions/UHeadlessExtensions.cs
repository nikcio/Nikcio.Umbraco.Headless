using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.Reflection;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.ContentItems.NotificationHandlers;
using Nikcio.UHeadless.Extensions.Options;
using Nikcio.UHeadless.MediaItems;
using Nikcio.UHeadless.MediaItems.NotficationHandlers;
using Nikcio.UHeadless.Members;
using Nikcio.UHeadless.Members.NotificationHandlers;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless.Extensions;

/// <summary>
/// The default UHeadless extensions used for the default setup
/// </summary>
public static class UHeadlessExtensions
{
    /// <summary>
    /// Adds all services the UHeadless package needs
    /// </summary>
    /// <param name="builder">The Umbraco builder</param>
    /// <returns></returns>
    public static IUmbracoBuilder AddUHeadless(this IUmbracoBuilder builder)
    {
        var uHeadlessOptions = new UHeadlessOptions();
        return AddUHeadless(builder, uHeadlessOptions);
    }

    /// <summary>
    /// Adds all services the UHeadless package needs
    /// </summary>
    /// <param name="builder">The Umbraco builder</param>
    /// <param name="uHeadlessOptions"></param>
    /// <returns></returns>
    public static IUmbracoBuilder AddUHeadless(this IUmbracoBuilder builder, UHeadlessOptions uHeadlessOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(uHeadlessOptions);

        builder.Services.AddScoped<IDependencyReflectorFactory, DependencyReflectorFactory>();
        builder.Services.AddScoped(typeof(IContentItemRepository<>), typeof(ContentItemRepository<>));
        builder.Services.AddScoped(typeof(IMediaItemRepository<>), typeof(MediaItemRepository<>));
        builder.Services.AddScoped(typeof(IMemberRepository<>), typeof(MemberRepository<>));
        builder.Services.AddSingleton(uHeadlessOptions.PropertyServicesOptions.PropertyMapOptions.PropertyMap);

        if (uHeadlessOptions.PropertyServicesOptions.PropertyMapOptions.PropertyMappings != null)
        {
            foreach (Action<IPropertyMap> propertyMapping in uHeadlessOptions.PropertyServicesOptions.PropertyMapOptions.PropertyMappings)
            {
                propertyMapping.Invoke(uHeadlessOptions.PropertyServicesOptions.PropertyMapOptions.PropertyMap);
            }
        }

        builder.AddNotificationAsyncHandler<ContentTypeChangedNotification, ContentTypeChangedHandler>();
        builder.Services.AddSingleton<ContentTypeModule>();
        builder.Services.AddSingleton<MediaTypeModule>();
        builder.AddNotificationAsyncHandler<MediaTypeChangedNotification, MediaTypeChangedHandler>();
        builder.Services.AddSingleton<MemberTypeModule>();
        builder.AddNotificationAsyncHandler<MemberTypeChangedNotification, MemberTypeChangedHandler>();
        //builder.Services
        //    .AddPropertyServices(uHeadlessOptions.PropertyServicesOptions)
        //    .AddContentTypeServices();

        //if (uHeadlessOptions.UHeadlessGraphQLOptions.GraphQLExtensions == null)
        //{
        //    uHeadlessOptions.UHeadlessGraphQLOptions.GraphQLExtensions = (builder) =>
        //        builder
        //            .UseContentQueries()
        //            .AddTypeExtension<BasicContentAtRootQuery>();
        //}

        uHeadlessOptions.PropertyServicesOptions.PropertyMapOptions.PropertyMap.AddPropertyMapDefaults();
        IEnumerable<Type> propertyValueTypes = uHeadlessOptions.PropertyServicesOptions.PropertyMapOptions.PropertyMap.GetAllTypes();

        uHeadlessOptions.UHeadlessGraphQLOptions.PropertyValueTypes.AddRange(propertyValueTypes);

        builder.Services
            .AddGraphQLServer()
            .AddUHeadlessGraphQL(uHeadlessOptions.UHeadlessGraphQLOptions)
            .AddTracing(uHeadlessOptions.TracingOptions);

        //builder.AddUHeadlessComposers();

        return builder;
    }

    /// <summary>
    /// Creates a GraphQL endpoint at the graphQlPath or "/graphql" by default
    /// </summary>
    /// <param name="applicationBuilder">The application builder</param>
    /// <returns></returns>
    [Obsolete("[From Umbraco v13] Use MapUHeadlessGraphQLEndpoint(this WebApplication app) instead")]
    public static IApplicationBuilder UseUHeadlessGraphQLEndpoint(this IApplicationBuilder applicationBuilder)
    {
        var uHeadlessEndpointOptions = new UHeadlessEndpointOptions();
        return UseUHeadlessGraphQLEndpoint(applicationBuilder, uHeadlessEndpointOptions);
    }

    /// <summary>
    /// Creates a GraphQL endpoint at the graphQlPath or "/graphql" by default
    /// </summary>
    /// <param name="applicationBuilder">The application builder</param>
    /// <param name="uHeadlessEndpointOptions"></param>
    /// <returns></returns>
    [Obsolete("[From Umbraco v13] Use MapUHeadlessGraphQLEndpoint(this WebApplication app, UHeadlessEndpointOptions uHeadlessEndpointOptions) instead")]
    public static IApplicationBuilder UseUHeadlessGraphQLEndpoint(this IApplicationBuilder applicationBuilder, UHeadlessEndpointOptions uHeadlessEndpointOptions)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);
        ArgumentNullException.ThrowIfNull(uHeadlessEndpointOptions);

        applicationBuilder.UseRouting();

        if (uHeadlessEndpointOptions.CorsPolicy != null)
        {
            applicationBuilder.UseCors(uHeadlessEndpointOptions.CorsPolicy);
        }
        else
        {
            applicationBuilder.UseCors();
        }

        applicationBuilder
            .UseEndpoints(endpoints => endpoints.MapGraphQL(uHeadlessEndpointOptions.GraphQLPath).WithOptions(uHeadlessEndpointOptions.GraphQLServerOptions));
        return applicationBuilder;
    }

    /// <summary>
    /// Creates a GraphQL endpoint at the graphQlPath or "/graphql" by default
    /// </summary>
    /// <param name="app">The web application</param>
    /// <returns></returns>
    public static WebApplication MapUHeadlessGraphQLEndpoint(this WebApplication app)
    {
        var uHeadlessEndpointOptions = new UHeadlessEndpointOptions();
        return MapUHeadlessGraphQLEndpoint(app, uHeadlessEndpointOptions);
    }

    /// <summary>
    /// Creates a GraphQL endpoint at the graphQlPath or "/graphql" by default
    /// </summary>
    /// <param name="app">The web application</param>
    /// <param name="uHeadlessEndpointOptions"></param>
    /// <returns></returns>
    public static WebApplication MapUHeadlessGraphQLEndpoint(this WebApplication app, UHeadlessEndpointOptions uHeadlessEndpointOptions)
    {
        ArgumentNullException.ThrowIfNull(uHeadlessEndpointOptions);

        if (uHeadlessEndpointOptions.CorsPolicy != null)
        {
            app.UseCors(uHeadlessEndpointOptions.CorsPolicy);
        }
        else
        {
            app.UseCors();
        }

        app.MapGraphQL(uHeadlessEndpointOptions.GraphQLPath).WithOptions(uHeadlessEndpointOptions.GraphQLServerOptions);
        return app;
    }
}
