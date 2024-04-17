using Nikcio.UHeadless.Common.Properties.Models;
using Umbraco.Cms.Core;

namespace Nikcio.UHeadless.Common.Properties;

/// <summary>
/// Extensions
/// </summary>
internal static class PropertyMapExtensions
{
    /// <summary>
    /// Adds the default mappings to the property map
    /// </summary>
    public static void AddPropertyMapDefaults(this IPropertyMap propertyMap)
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
