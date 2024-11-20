using HotChocolate.Resolvers;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models;

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
[Obsolete("Use Defaults.Properties.NestedContent instead.")]
public class BasicNestedContent : BasicNestedContent<BasicNestedContentElement>
{
    public BasicNestedContent(CreateCommand command) : base(command)
    {
    }

    protected override BasicNestedContentElement CreateNestedContentItem(IPublishedElement publishedElement, IResolverContext resolverContext)
    {
        return new BasicNestedContentElement(publishedElement, resolverContext);
    }
}

/// <summary>
/// Represents nested content
/// </summary>
/// <typeparam name="TNestedContentElement"></typeparam>
[GraphQLDescription("Represents nested content.")]
[Obsolete("Use Defaults.Properties.NestedContent instead.")]
public abstract class BasicNestedContent<TNestedContentElement> : NestedContent<TNestedContentElement>
    where TNestedContentElement : class
{
    protected BasicNestedContent(CreateCommand command) : base(command)
    {
    }
}
