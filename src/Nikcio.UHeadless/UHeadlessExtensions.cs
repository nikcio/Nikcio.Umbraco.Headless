using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults;
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
            UmbracoBuilder = builder
        };
        configure?.Invoke(options);

        builder.Services.AddScoped<IDependencyReflectorFactory, DependencyReflectorFactory>();
        builder.Services.AddSingleton<UmbracoTypeModule>();
        builder.Services.AddSingleton(options.PropertyMap);

        builder.Services.AddScoped(typeof(IContentItemRepository<>), typeof(ContentItemRepository<>));
        builder.AddNotificationAsyncHandler<ContentTypeChangedNotification, ContentTypeChangedHandler>();

        builder.Services.AddScoped(typeof(IMediaItemRepository<>), typeof(MediaItemRepository<>));
        builder.AddNotificationAsyncHandler<MediaTypeChangedNotification, MediaTypeChangedHandler>();

        builder.Services.AddScoped(typeof(IMemberItemRepository<>), typeof(MemberItemRepository<>));
        builder.AddNotificationAsyncHandler<MemberTypeChangedNotification, MemberTypeChangedHandler>();

        requestExecutorBuilder
            .InitializeOnStartup()
            .AddAuthorization()
            .AddQueryType<HotChocolateQueryObject>()
            .AddInterfaceType<PropertyValue>()
            .AddTypeModule<UmbracoTypeModule>();

        foreach (Type type in options.PropertyMap.GetAllTypes())
        {
            requestExecutorBuilder.AddType(type);
        }

        return builder;
    }

    /// <summary>
    /// Adds default property mappings and services to be used for the default queries.
    /// </summary>
    public static void AddDefaults(this UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AddDefaultsInternal();
    }

    /// <summary>
    /// Adds a query to the GraphQL schema
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    public static UHeadlessOptions AddQuery<TQuery>(this UHeadlessOptions options)
        where TQuery : class, IGraphQLQuery, new()
    {
        ArgumentNullException.ThrowIfNull(options);

        options.RequestExecutorBuilder.AddTypeExtension<TQuery>();

        var query = new TQuery();
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
