using Nikcio.UHeadless.Base.Basics.EditorsValues.BlockGrid.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.BlockList.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.DateTimePicker.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.Fallback.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.Labels.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.MemberPicker.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.MultiUrlPicker.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models;
using Nikcio.UHeadless.Base.Basics.EditorsValues.RichTextEditor.Models;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core;

namespace Nikcio.UHeadless.Base.Basics.Maps.Extensions;

/// <summary>
/// Extensions
/// </summary>
public static class PropertyMapExtensions
{
    /// <summary>
    /// Adds the default mappings to the property map
    /// </summary>
    [Obsolete("Convert to using .AddDefaults() instead after checking the models for breaking changes.")]
    public static void AddPropertyMapDefaults(this IPropertyMap propertyMap)
    {
        ArgumentNullException.ThrowIfNull(propertyMap);

        propertyMap.AddEditorMapping<BasicPropertyValue>(PropertyConstants.DefaultKey);
        propertyMap.AddEditorMapping<BasicBlockListModel>(Constants.PropertyEditors.Aliases.BlockList);
        propertyMap.AddEditorMapping<BasicBlockGridModel>(Constants.PropertyEditors.Aliases.BlockGrid);
        propertyMap.AddEditorMapping<BasicNestedContent>(Constants.PropertyEditors.Aliases.NestedContent);
        propertyMap.AddEditorMapping<BasicRichText>(Constants.PropertyEditors.Aliases.TinyMce);
        propertyMap.AddEditorMapping<BasicRichText>(Constants.PropertyEditors.Aliases.MarkdownEditor);
        propertyMap.AddEditorMapping<BasicMemberPicker>(Constants.PropertyEditors.Aliases.MemberPicker);
        propertyMap.AddEditorMapping<BasicContentPicker>(Constants.PropertyEditors.Aliases.ContentPicker);
        propertyMap.AddEditorMapping<BasicMultiUrlPicker>(Constants.PropertyEditors.Aliases.MultiUrlPicker);
        propertyMap.AddEditorMapping<BasicContentPicker>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        propertyMap.AddEditorMapping<BasicContentPicker>(Constants.PropertyEditors.Aliases.MultiNodeTreePicker);
        propertyMap.AddEditorMapping<BasicMediaPicker>(Constants.PropertyEditors.Aliases.MediaPicker3);
        propertyMap.AddEditorMapping<BasicMediaPicker>(Constants.PropertyEditors.Aliases.MultipleMediaPicker);
        propertyMap.AddEditorMapping<BasicDateTimePicker>(Constants.PropertyEditors.Aliases.DateTime);
        propertyMap.AddEditorMapping<BasicLabel>(Constants.PropertyEditors.Aliases.Label);
        propertyMap.AddEditorMapping<BasicUnsupportedPropertyValue>(Constants.PropertyEditors.Aliases.Grid);
    }
}
