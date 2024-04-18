using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.ContentPicker.Models;

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
[Obsolete("Use Defaults.Properties.ContentPicker instead.")]
public class BasicContentPicker : BasicContentPicker<BasicContentPickerItem>
{
    public BasicContentPicker(CreateCommand command) : base(command)
    {
    }

    protected override BasicContentPickerItem CreateContentPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext)
    {
        return new BasicContentPickerItem(publishedContent, resolverContext);
    }
}

/// <summary>
/// Represents a content picker value
/// </summary>
[GraphQLDescription("Represents a content picker value.")]
[Obsolete("Use Defaults.Properties.ContentPicker instead.")]
public abstract class BasicContentPicker<TContentPickerItem> : ContentPicker<BasicContentPickerItem>
    where TContentPickerItem : class
{
    protected BasicContentPicker(CreateCommand command) : base(command)
    {
    }
}
