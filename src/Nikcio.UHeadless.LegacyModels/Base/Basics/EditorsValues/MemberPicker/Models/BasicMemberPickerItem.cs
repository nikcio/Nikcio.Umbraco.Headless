using HotChocolate;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Defaults.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Base.Basics.EditorsValues.MemberPicker.Models;

/// <inheritdoc/>
[GraphQLDescription("Represents a member item.")]
[Obsolete("Use Defaults.Properties.MemberPickerItem instead. The main difference here is that the properties are typed which means you have to specify what data you need in the graphql query.")]
public class BasicMemberPickerItem : BasicMemberPickerItem<BasicProperty>
{
    public BasicMemberPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext) : base(publishedContent, resolverContext)
    {
    }
}

/// <inheritdoc/>
[GraphQLDescription("Represents a member item.")]
[Obsolete("Use Defaults.Properties.MemberPickerItem instead. The main difference here is that the properties are typed which means you have to specify what data you need in the graphql query.")]
public class BasicMemberPickerItem<TProperty> : MemberPickerItem
    where TProperty : IProperty
{
    public BasicMemberPickerItem(IPublishedContent publishedContent, IResolverContext resolverContext) : base(publishedContent, resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);
        ArgumentNullException.ThrowIfNull(publishedContent);

        IPropertyFactory<TProperty> propertyFactory = resolverContext.Service<IPropertyFactory<TProperty>>();

        IPublishedContent? contentElement = resolverContext.GetScopedState<IPublishedContent>(ContextDataKeys.PublishedContent);
        string? culture = resolverContext.GetScopedState<string?>(ContextDataKeys.Culture);
        string? segment = resolverContext.GetScopedState<string?>(ContextDataKeys.Segment);
        Umbraco.Cms.Core.Models.PublishedContent.Fallback fallback = resolverContext.GetScopedState<Umbraco.Cms.Core.Models.PublishedContent.Fallback>(ContextDataKeys.Fallback);

        if (contentElement == null)
        {
            return;
        }

        foreach (IPublishedProperty property in publishedContent.Properties)
        {
            Properties.Add(propertyFactory.GetProperty(property, contentElement, culture, segment, fallback));
        }
    }

    /// <inheritdoc/>
    [GraphQLDescription("Gets the properties of a member.")]
    [Obsolete("Transition to typed properties instead using the Defaults.Properties.MemberPickerItem model.")]
    public new List<TProperty?> Properties { get; set; } = new();
}
