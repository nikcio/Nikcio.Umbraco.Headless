using HotChocolate;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.ContentTypes.Basics.Models;
using Nikcio.UHeadless.Defaults.Members;
using Nikcio.UHeadless.Properties;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Members.Basics.Models;

/// <summary>
/// Represents a member
/// </summary>
[Obsolete("Convert to using Defaults.MemberItems.MemberItem or a custom model if you need special properties.")]
public class BasicMember : BasicMember<BasicProperty>
{
    public BasicMember(CreateCommand command) : base(command)
    {
    }
}

/// <summary>
/// Represents a member
/// </summary>
[Obsolete("Convert to using Defaults.MemberItems.MemberItem or a custom model if you need special properties.")]
public class BasicMember<TProperty> : MemberItem
    where TProperty : IProperty
{
    public BasicMember(CreateCommand command) : base(command)
    {
    }

    /// <summary>
    /// Gets the type of the content item (document, media...)
    /// </summary>
    [GraphQLDescription("Gets the type of the content item (document, media...).")]
    [Obsolete("If you need this add it to your own model")]
    public virtual PublishedItemType? ItemType => PublishedContent?.ItemType;

    /// <summary>
    /// Gets available culture infos
    /// </summary>
    [GraphQLDescription("Gets available culture infos.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual IReadOnlyDictionary<string, PublishedCultureInfo>? Cultures => PublishedContent?.Cultures;

    /// <summary>
    /// Gets the identifier of the user who last updated the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the user who last updated the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? WriterId => PublishedContent?.WriterId;

    /// <summary>
    /// Gets the date that the content was created
    /// </summary>
    [GraphQLDescription("Gets the date that the content was created.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual DateTime? CreateDate => PublishedContent?.CreateDate;

    /// <summary>
    /// Gets the identifier of the user who created the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the user who created the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? CreatorId => PublishedContent?.CreatorId;

    /// <summary>
    /// Gets the tree path of the content item
    /// </summary>
    [GraphQLDescription("Gets the tree path of the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual string? Path => PublishedContent?.Path;

    /// <summary>
    /// Gets the tree level of the content item
    /// </summary>
    [GraphQLDescription("Gets the tree level of the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? Level => PublishedContent?.Level;

    /// <summary>
    /// Gets the sort order of the content item
    /// </summary>
    [GraphQLDescription("Gets the sort order of the content item.")]
    [Obsolete("If you need this add it to your own model")]
    public virtual int? SortOrder => PublishedContent?.SortOrder;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the content type.")]
    [Obsolete("Use your own model here.")]
    public virtual BasicContentType? ContentType => PublishedContent != null ? new BasicContentType(PublishedContent.ContentType) : default;

    /// <inheritdoc/>
    [GraphQLDescription("Gets the properties of the element.")]
    [Obsolete("Use typed properties instead.")]
    public new IEnumerable<TProperty?>? Properties => PublishedContent != null ? ResolverContext.Service<IPropertyFactory<TProperty>>().CreateProperties(PublishedContent, Culture, Segment, ResolverContext.GetScopedState<Fallback?>(ContextDataKeys.Fallback)) : default;

    /// <summary>
    /// Gets the named properties of the element using the content types in Umbraco
    /// </summary>
    [GraphQLDescription("Gets the named properties of the element using the content types in Umbraco.")]
    [Obsolete("Use typed properties on the Defaults.ContentItems.ContentItem")]
    public TypedProperties NamedProperties()
    {
        ResolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }
}
