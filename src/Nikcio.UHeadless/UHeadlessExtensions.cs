using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Common.Directives;
using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Common.Reflection;
using Nikcio.UHeadless.Common.TypeModules;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Defaults.Properties;
using Nikcio.UHeadless.MediaItems;
using Nikcio.UHeadless.MemberItems;
using Umbraco.Cms.Core;
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
        ArgumentNullException.ThrowIfNull(propertyMap);

        propertyMap.AddEditorMapping<DefaultProperty>(PropertyConstants.DefaultKey);
        propertyMap.AddEditorMapping<BlockList>(Constants.PropertyEditors.Aliases.BlockList);
        propertyMap.AddEditorMapping<BlockGrid>(Constants.PropertyEditors.Aliases.BlockGrid);
        propertyMap.AddEditorMapping<ContentPicker>(Constants.PropertyEditors.Aliases.ContentPicker);
        propertyMap.AddEditorMapping<ContentPicker>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        propertyMap.AddEditorMapping<ContentPicker>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        propertyMap.AddEditorMapping<NestedContent>(Constants.PropertyEditors.Aliases.NestedContent);
        propertyMap.AddEditorMapping<RichText>(Constants.PropertyEditors.Aliases.TinyMce);
        propertyMap.AddEditorMapping<RichText>(Constants.PropertyEditors.Aliases.MarkdownEditor);
        propertyMap.AddEditorMapping<MemberPicker>(Constants.PropertyEditors.Aliases.MemberPicker);
        propertyMap.AddEditorMapping<MultiUrlPicker>(Constants.PropertyEditors.Aliases.MultiUrlPicker);
        propertyMap.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MediaPicker);
        propertyMap.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MediaPicker3);
        propertyMap.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MultipleMediaPicker);
        propertyMap.AddEditorMapping<DateTimePicker>(Constants.PropertyEditors.Aliases.DateTime);
        propertyMap.AddEditorMapping<Label>(Constants.PropertyEditors.Aliases.Label);
        propertyMap.AddEditorMapping<UnsupportedProperty>(Constants.PropertyEditors.Aliases.Grid);
    }
}
