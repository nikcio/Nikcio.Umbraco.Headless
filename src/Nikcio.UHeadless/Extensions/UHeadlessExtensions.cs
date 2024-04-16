using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Common.Directives;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.Reflection;
using Nikcio.UHeadless.Common.TypeModules;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.MediaItems;
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
    /// <param name="configure">Configures the options used by UHeadless.</param>
    /// <returns></returns>
    public static IUmbracoBuilder AddUHeadless(this IUmbracoBuilder builder, Action<UHeadlessOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        IRequestExecutorBuilder requestExecutorBuilder = builder.Services.AddGraphQLServer();

        var options = new UHeadlessOptions()
        {
            RequestExecutorBuilder = requestExecutorBuilder
        };
        configure?.Invoke(options);

        builder.Services.AddScoped<IDependencyReflectorFactory, DependencyReflectorFactory>();
        builder.Services.AddSingleton<UmbracoTypeModule>();
        builder.Services.AddSingleton(options.PropertyMap);

        builder.Services.AddScoped(typeof(IContentItemRepository<>), typeof(ContentItemRepository<>));
        builder.AddNotificationAsyncHandler<ContentTypeChangedNotification, ContentTypeChangedHandler>();

        builder.Services.AddScoped(typeof(IMediaItemRepository<>), typeof(MediaItemRepository<>));
        builder.AddNotificationAsyncHandler<MediaTypeChangedNotification, MediaTypeChangedHandler>();

        builder.Services.AddScoped(typeof(IMemberRepository<>), typeof(MemberRepository<>));
        builder.AddNotificationAsyncHandler<MemberTypeChangedNotification, MemberTypeChangedHandler>();

        requestExecutorBuilder
            .InitializeOnStartup()
            .AddFiltering()
            .AddSorting()
            .AddQueryType<GraphQLQuery>()
            .AddInterfaceType<PropertyValue>()
            .AddTypeModule<UmbracoTypeModule>()
            .AddDirectiveType<ContextDirective>()
            .AddDirectiveType<FallbackDirective>()
            .AddDirectiveType<SegmentDirective>();

        foreach (Type type in options.PropertyMap.GetAllTypes())
        {
            requestExecutorBuilder.AddType(type);
        }

        return builder;
    }

    /// <summary>
    /// Adds default property mappings to the property map
    /// </summary>
    /// <param name="propertyMap"></param>
    public static void AddDefaults(this IPropertyMap propertyMap)
    {
        propertyMap.AddPropertyMapDefaults();
    }
}
