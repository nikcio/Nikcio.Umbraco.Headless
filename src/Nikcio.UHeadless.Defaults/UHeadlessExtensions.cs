using Nikcio.UHeadless.Common.Properties;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core;

namespace Nikcio.UHeadless.Defaults;

public static class UHeadlessExtensions
{
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
