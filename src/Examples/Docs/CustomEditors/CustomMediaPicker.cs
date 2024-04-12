using Nikcio.UHeadless.Common.Properties.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Examples.Docs.CustomEditors;

public class CustomMediaPicker : MediaPickerResponse
{
    public string MyCustomProperty { get; set; }

    public CustomMediaPicker(CreateCommand command, IVariationContextAccessor variationContextAccessor) : base(command, variationContextAccessor)
    {
        MyCustomProperty = "Here's a custom property";
    }
}
