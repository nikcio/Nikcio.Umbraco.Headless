using Nikcio.UHeadless.Base.Basics.EditorsValues.MediaPicker.Models;
using Nikcio.UHeadless.Base.Properties.EditorsValues.MediaPicker.Commands;

namespace Examples.Docs.CustomEditors;

public class MyMediaPickerItem : BasicMediaPickerItem
{
    public int MyProperty { get; set; }

    public MyMediaPickerItem(CreateMediaPickerItem createMediaPickerItem) : base(createMediaPickerItem)
    {
        MyProperty = 0;
    }
}
