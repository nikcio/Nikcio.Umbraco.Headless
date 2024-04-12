using Nikcio.UHeadless.Common.Properties.Models;
using Umbraco.Cms.Core;

namespace Nikcio.UHeadless.Common.Properties;

/// <summary>
/// Extensions
/// </summary>
public static class PropertyMapExtensions
{
    /// <summary>
    /// Adds the default mappings to the property map
    /// </summary>
    public static void AddPropertyMapDefaults(this IPropertyMap propertyMap)
    {
        ArgumentNullException.ThrowIfNull(propertyMap);

        propertyMap.AddEditorMapping<DefaultPropertyResponse>(PropertyConstants.DefaultKey);
        propertyMap.AddEditorMapping<BlockListResponse>(Constants.PropertyEditors.Aliases.BlockList);
        propertyMap.AddEditorMapping<BlockGridResponse>(Constants.PropertyEditors.Aliases.BlockGrid);
        propertyMap.AddEditorMapping<ContentPickerResponse>(Constants.PropertyEditors.Aliases.ContentPicker);
        propertyMap.AddEditorMapping<ContentPickerResponse>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        propertyMap.AddEditorMapping<ContentPickerResponse>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        propertyMap.AddEditorMapping<NestedContentResponse>(Constants.PropertyEditors.Aliases.NestedContent);
        propertyMap.AddEditorMapping<RichTextResponse>(Constants.PropertyEditors.Aliases.TinyMce);
        propertyMap.AddEditorMapping<RichTextResponse>(Constants.PropertyEditors.Aliases.MarkdownEditor);
        propertyMap.AddEditorMapping<MemberPickerResponse>(Constants.PropertyEditors.Aliases.MemberPicker);
        propertyMap.AddEditorMapping<MultiUrlPickerResponse>(Constants.PropertyEditors.Aliases.MultiUrlPicker);
        propertyMap.AddEditorMapping<MediaPickerResponse>(Constants.PropertyEditors.Aliases.MediaPicker);
        propertyMap.AddEditorMapping<MediaPickerResponse>(Constants.PropertyEditors.Aliases.MediaPicker3);
        propertyMap.AddEditorMapping<MediaPickerResponse>(Constants.PropertyEditors.Aliases.MultipleMediaPicker);
        propertyMap.AddEditorMapping<DateTimePickerResponse>(Constants.PropertyEditors.Aliases.DateTime);
        propertyMap.AddEditorMapping<LabelResponse>(Constants.PropertyEditors.Aliases.Label);
        propertyMap.AddEditorMapping<UnsupportedResponse>(Constants.PropertyEditors.Aliases.Grid);
    }
}
