using System.ComponentModel.DataAnnotations;
using HotChocolate.AspNetCore.Extensions;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults;
using Nikcio.UHeadless.Defaults.Authorization;
using Nikcio.UHeadless.MediaItems;
using Nikcio.UHeadless.MemberItems;
using Nikcio.UHeadless.Reflection;
using Nikcio.UHeadless.TypeModules;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace Nikcio.UHeadless;

/// <summary>
/// The UHeadless extensions
/// </summary>
public static class UHeadlessExtensions
{
    /// <summary>
    /// Adds all services the UHeadless package needs
    /// </summary>
    /// <param name="builder">The Umbraco builder</param>
    /// <param name="configure">Configures the options used by UHeadless.</param>
    /// <returns></returns>
    public static IUmbracoBuilder AddUHeadless(this IUmbracoBuilder builder, Action<UHeadlessOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        IRequestExecutorBuilder requestExecutorBuilder = builder.Services.AddGraphQLServer();

        var options = new UHeadlessOptions()
        {
            RequestExecutorBuilder = requestExecutorBuilder,
            UmbracoBuilder = builder,
        };
        configure?.Invoke(options);

        if (options.DisableAuthorization == false && options.AuthorizationOptions == null)
        {
            throw new InvalidOperationException("Authorization options must be set when authorization is enabled. Set the options in the UHeadless options using .AddAuth().");
        }

        if (options.DisableAuthorization && options.AuthorizationOptions != null)
        {
            throw new InvalidOperationException("Authorization options are set but authorization is disabled. Remove .AddAuth() to run without authorization.");
        }

        if (options.AuthorizationOptions != null)
        {
            Validator.ValidateObject(options.AuthorizationOptions, new ValidationContext(options.AuthorizationOptions), true);
        }

        builder.Services.AddSingleton(options.AuthorizationOptions ?? new()
        {
            ApiKey = string.Empty,
            Secret = string.Empty,
            IsAuthorizationDisabled = true,
        });

        builder.Services.AddScoped<IDependencyReflectorFactory, DependencyReflectorFactory>();
        builder.Services.AddSingleton<UmbracoTypeModule>();
        builder.Services.AddSingleton(options.PropertyMap);
        builder.Services.AddSingleton<IAuthorizationHandler, AlwaysAllowAuthorizationHandler>();

        builder.Services.AddScoped(typeof(IContentItemRepository<>), typeof(ContentItemRepository<>));
        builder.AddNotificationAsyncHandler<ContentTypeChangedNotification, ContentTypeChangedHandler>();

        builder.Services.AddScoped(typeof(IMediaItemRepository<>), typeof(MediaItemRepository<>));
        builder.AddNotificationAsyncHandler<MediaTypeChangedNotification, MediaTypeChangedHandler>();

        builder.Services.AddScoped(typeof(IMemberItemRepository<>), typeof(MemberItemRepository<>));
        builder.AddNotificationAsyncHandler<MemberTypeChangedNotification, MemberTypeChangedHandler>();

        requestExecutorBuilder
            .InitializeOnStartup()
            .AddAuthorization()
            .AddInterfaceType<PropertyValue>()
            .AddTypeModule<UmbracoTypeModule>();

        if (options.HasQueries)
        {
            requestExecutorBuilder.AddQueryType<HotChocolateQueryObject>();
        }

        if (options.HasMutations)
        {
            requestExecutorBuilder.AddMutationType<HotChocolateMutationObject>();
        }

        foreach (Type type in options.PropertyMap.GetAllTypes())
        {
            requestExecutorBuilder.AddType(type);
        }

        return builder;
    }

    /// <summary>
    /// Maps the UHeadless GraphQL endpoint
    /// </summary>
    /// <param name="app"></param>
    /// <param name="path">The path to which the GraphQL endpoint shall be mapped.</param>
    /// <param name="schemaName">The name of the schema that shall be used by this endpoint.</param>
    public static GraphQLEndpointConventionBuilder MapUHeadless(this WebApplication app, string path = "/graphql", string? schemaName = null)
    {
        ArgumentNullException.ThrowIfNull(app);

        UHeadlessAuthorizationOptions authorizationOptions = app.Services.GetRequiredService<UHeadlessAuthorizationOptions>();

        if (authorizationOptions.IsAuthorizationDisabled)
        {
            return app.MapGraphQL(path, schemaName);
        }

        return app.MapGraphQL(path, schemaName).RequireAuthorization(policy =>
        {
            policy.AddAuthenticationSchemes(DefaultAuthenticationSchemes.UHeadless, DefaultAuthenticationSchemes.UHeadlessApiKey);
            policy.AddRequirements(new AlwaysAllowAuthoriaztionRequirement());
        });
    }

    /// <summary>
    /// Adds default property mappings and services to be used for the default queries.
    /// </summary>
    public static UHeadlessOptions AddDefaults(this UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return options.AddDefaultsInternal();
    }

    /// <summary>
    /// Adds the authentication services to the UHeadless package.
    /// </summary>
    /// <remarks>
    /// This is required if <see cref="UHeadlessOptions.DisableAuthorization"/> is set to false.
    /// </remarks>
    public static UHeadlessOptions AddAuth(this UHeadlessOptions options, UHeadlessAuthorizationOptions authorizationOptions)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(authorizationOptions);

        options.AuthorizationOptions = authorizationOptions;

        if (options.DisableAuthorization)
        {
            throw new InvalidOperationException("Authorization is disabled. If you want to use the built-in auth services you need to enable authorization.");
        }

        return options.AddAuthInternal();
    }

    /// <summary>
    /// Adds a query to the GraphQL schema
    /// </summary>
    public static UHeadlessOptions AddQuery<TQuery>(this UHeadlessOptions options)
        where TQuery : class, IGraphQLQuery, new()
    {
        ArgumentNullException.ThrowIfNull(options);

        options.HasQueries = true;

        options.RequestExecutorBuilder.AddTypeExtension<TQuery>();

        var query = new TQuery();
        query.ApplyConfiguration(options);

        return options;
    }

    /// <summary>
    /// Adds a mutation to the GraphQL schema
    /// </summary>
    public static UHeadlessOptions AddMutation<TMutation>(this UHeadlessOptions options)
        where TMutation : class, IGraphQLMutation, new()
    {
        ArgumentNullException.ThrowIfNull(options);

        options.HasMutations = true;

        options.RequestExecutorBuilder.AddTypeExtension<TMutation>();

        var query = new TMutation();
        query.ApplyConfiguration(options);

        return options;
    }

    /// <summary>
    /// Adds a mapping of a type to a editor alias.
    /// </summary>
    /// <typeparam name="TType">The type that should be used for this property</typeparam>
    public static UHeadlessOptions AddEditorMapping<TType>(this UHeadlessOptions options, string editorName)
        where TType : PropertyValue
    {
        ArgumentNullException.ThrowIfNull(options);

        options.PropertyMap.AddEditorMapping<TType>(editorName);

        return options;
    }

    /// <summary>
    /// Adds a mapping of a type to a content type alias combined with a property type alias.
    /// </summary>
    /// <typeparam name="TType">The type that should be used for this property</typeparam>
    /// <example>
    /// ContentTypeAlias: MyDocType
    /// PropertyTypeAlias: MyProperty
    /// </example>
    /// <remarks>This takes precedence over editor mappings</remarks>
    public static UHeadlessOptions AddAliasMapping<TType>(this UHeadlessOptions options, string contentTypeAlias, string propertyTypeAlias)
        where TType : PropertyValue
    {
        ArgumentNullException.ThrowIfNull(options);

        options.PropertyMap.AddAliasMapping<TType>(contentTypeAlias, propertyTypeAlias);

        return options;
    }

    /// <summary>
    /// Removes a alias mapping
    /// </summary>
    /// <example>
    /// ContentTypeAlias: MyDocType
    /// PropertyTypeAlias: MyProperty
    /// </example>
    public static UHeadlessOptions RemoveAliasMapping(this UHeadlessOptions options, string contentTypeAlias, string propertyTypeAlias)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.PropertyMap.RemoveAliasMapping(contentTypeAlias, propertyTypeAlias);

        return options;
    }

    /// <summary>
    /// Removes a editor mapping
    /// </summary>
    public static UHeadlessOptions RemoveEditorMapping(this UHeadlessOptions options, string editorName)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.PropertyMap.RemoveEditorMapping(editorName);

        return options;
    }
}
