using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.Defaults.Properties;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core;

namespace Nikcio.UHeadless.Defaults;

internal static class Bootstrap
{
    /// <summary>
    /// Adds default property mappings and services to be used for the default queries.
    /// </summary>
    public static void AddDefaultsInternal(this UHeadlessOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.PropertyMap.AddEditorMapping<DefaultProperty>(PropertyConstants.DefaultKey);
        options.PropertyMap.AddEditorMapping<BlockList>(Constants.PropertyEditors.Aliases.BlockList);
        options.PropertyMap.AddEditorMapping<BlockGrid>(Constants.PropertyEditors.Aliases.BlockGrid);
        options.PropertyMap.AddEditorMapping<ContentPicker>(Constants.PropertyEditors.Aliases.ContentPicker);
        options.PropertyMap.AddEditorMapping<ContentPicker>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        options.PropertyMap.AddEditorMapping<ContentPicker>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        options.PropertyMap.AddEditorMapping<NestedContent>(Constants.PropertyEditors.Aliases.NestedContent);
        options.PropertyMap.AddEditorMapping<RichText>(Constants.PropertyEditors.Aliases.TinyMce);
        options.PropertyMap.AddEditorMapping<RichText>(Constants.PropertyEditors.Aliases.MarkdownEditor);
        options.PropertyMap.AddEditorMapping<MultiUrlPicker>(Constants.PropertyEditors.Aliases.MultiUrlPicker);
        options.PropertyMap.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MediaPicker);
        options.PropertyMap.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MediaPicker3);
        options.PropertyMap.AddEditorMapping<MediaPicker>(Constants.PropertyEditors.Aliases.MultipleMediaPicker);
        options.PropertyMap.AddEditorMapping<DateTimePicker>(Constants.PropertyEditors.Aliases.DateTime);
        options.PropertyMap.AddEditorMapping<Label>(Constants.PropertyEditors.Aliases.Label);
        options.PropertyMap.AddEditorMapping<UnsupportedProperty>(Constants.PropertyEditors.Aliases.Grid);
        options.PropertyMap.AddEditorMapping<MemberPicker>(Constants.PropertyEditors.Aliases.MemberPicker);
        MemberPicker.ApplyConfiguration(options);

        if (options.DisableAuthorization)
        {
            options.UmbracoBuilder.Services.AddSingleton<IAuthorizationHandler, AlwaysAllowAuthorizationHandler>();
        }
    }
}
