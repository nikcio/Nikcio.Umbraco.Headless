using Examples.Docs.Properties;
using Nikcio.UHeadless.Base.Basics.EditorsValues.BlockList.Models;
using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Common.Reflection;

namespace Examples.Docs.PropertyValues;

public class MyBlockListModel : BasicBlockListModel
{
    public string MyCustomProperty { get; set; }

    public MyBlockListModel(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
        MyCustomProperty = "Hello I'm Custom!";
    }
}

public class MyBlockListModelWithMyProperty : BasicBlockListModel<BasicBlockListItem<MyProperty>>
{
    public string MyCustomProperty { get; set; }

    public MyBlockListModelWithMyProperty(CreatePropertyValue createPropertyValue, IDependencyReflectorFactory dependencyReflectorFactory) : base(createPropertyValue, dependencyReflectorFactory)
    {
        MyCustomProperty = "Hello I'm Custom!";
    }
}