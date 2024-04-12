using Nikcio.UHeadless.Common.Properties.Models;

namespace Examples.Docs.CustomEditors;

public class CustomBlockListModel : BlockListResponse
{
    public string MyCustomProperty { get; set; }

    public CustomBlockListModel(CreateCommand comamnd) : base(comamnd)
    {
        MyCustomProperty = "Hello I'm Custom!";
    }
}
