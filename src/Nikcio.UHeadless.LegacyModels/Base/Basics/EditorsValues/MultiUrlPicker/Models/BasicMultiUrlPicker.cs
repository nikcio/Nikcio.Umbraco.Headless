using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MultiUrlPicker.Models;

/// <summary>
/// Represents a multi url picker
/// </summary>
[GraphQLDescription("Represents a multi url picker.")]
[Obsolete("Use Defaults.Properties.MultiUrlPicker instead.")]
public class BasicMultiUrlPicker : BasicMultiUrlPicker<BasicMultiUrlPickerItem>
{
    public BasicMultiUrlPicker(CreateCommand command) : base(command)
    {
    }

    protected override BasicMultiUrlPickerItem CreateMultiUrlPickerItem(IPublishedContent? publishedContent, Link link, IResolverContext resolverContext)
    {
        return new BasicMultiUrlPickerItem(publishedContent, link, resolverContext);
    }
}

/// <summary>
/// Represents a multi url picker
/// </summary>
[GraphQLDescription("Represents a multi url picker.")]
[Obsolete("Use Defaults.Properties.MultiUrlPicker instead.")]
public abstract class BasicMultiUrlPicker<TLink> : MultiUrlPicker<TLink>
    where TLink : class
{
    protected BasicMultiUrlPicker(CreateCommand command) : base(command)
    {
    }
}
