using Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;
using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Common.Reflection;

namespace Examples.Docs.PropertyValues;

public class MyMediaPicker : BasicMediaPicker
{
    public string MyCustomProperty { get; set; }

    public MyMediaPicker(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
        MyCustomProperty = "Here's a custom property";
    }
}

public class MyMediaPickerWithMyMediaPickerItem : BasicMediaPicker<MyMediaPickerItem>
{
    public string MyCustomProperty { get; set; }

    public MyMediaPickerWithMyMediaPickerItem(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
        MyCustomProperty = "Here's a custom property2";
    }
}
