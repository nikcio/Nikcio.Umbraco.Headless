using Nikcio.UHeadless.Base.Basics.EditorsValues.RichTextEditor.Models;
using Nikcio.UHeadless.Base.Properties.Commands;

namespace Examples.Docs.CustomEditors;

public class MyRichText : BasicRichText
{
    public string MyCustomProperty { get; set; }

    public MyRichText(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        MyCustomProperty = "Hello here is a property";
    }
}
