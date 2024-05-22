using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.NestedContent.Models;

/// <summary>
/// Represents nested content
/// </summary>
[GraphQLDescription("Represents nested content.")]
[Obsolete("Use Defaults.Properties.NestedContentItem instead. The main difference here is that the properties are typed and will need to be explicitly called upon in the GraphQL query.")]
public class BasicNestedContentElement : BasicNestedContentElement<BasicProperty>
{
    public BasicNestedContentElement(IPublishedElement publishedElement, IResolverContext resolverContext) : base(publishedElement, resolverContext)
    {
    }
}

/// <summary>
/// Represents nested content
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[GraphQLDescription("Represents nested content.")]
[Obsolete("Use Defaults.Properties.NestedContentItem instead. The main difference here is that the properties are typed and will need to be explicitly called upon in the GraphQL query.")]
public class BasicNestedContentElement<TProperty> : NestedContentItem
    where TProperty : IProperty
{
    public BasicNestedContentElement(IPublishedElement publishedElement, IResolverContext resolverContext) : base(publishedElement, resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        IPropertyFactory<TProperty> propertyFactory = resolverContext.Service<IPropertyFactory<TProperty>>();

        IPublishedContent? publishedContent = resolverContext.GetScopedState<IPublishedContent>(ContextDataKeys.PublishedContent);
        string? culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        string? segment = resolverContext.GetScopedState<string?>(ContextDataKeys.Segment);
        Umbraco.Cms.Core.Models.PublishedContent.Fallback fallback = resolverContext.GetScopedState<Umbraco.Cms.Core.Models.PublishedContent.Fallback>(ContextDataKeys.Fallback);

        if (publishedElement != null && publishedContent != null)
        {
            foreach (IPublishedProperty property in publishedElement.Properties)
            {
                Properties.Add(propertyFactory.GetProperty(property, publishedContent, culture, segment, fallback));
            }
        }
    }

    /// <summary>
    /// Gets the properties of the nested content
    /// </summary>
    [GraphQLDescription("Gets the properties of the nested content.")]
    [Obsolete("Transition to using typed properties by using the Defaults.Properties.NestedContentItem.")]
    public new List<TProperty?> Properties { get; set; } = [];
}
