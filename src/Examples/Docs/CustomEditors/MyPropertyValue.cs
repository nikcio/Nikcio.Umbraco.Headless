using System;
using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Examples.Docs.CustomEditors;

public class MyPropertyValue : PropertyValue
{
    public string? Name { get; set; }

    public MyPropertyValue(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
    {
        ArgumentNullException.ThrowIfNull(createPropertyValue);

        object? value = createPropertyValue.Property.GetValue(createPropertyValue.Culture);
        if (value == null)
        {
            return;
        }

        Name = (string) value;
    }
}
