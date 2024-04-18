using HotChocolate;
using Nikcio.UHeadless.ContentTypes.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentTypes.Basics.Models;

/// <summary>
/// Represents a content type
/// </summary>
[GraphQLDescription("Represents a content type.")]
public class BasicContentType : ContentType
{
    /// <inheritdoc/>
    public BasicContentType(IPublishedContentType contentType) : base(contentType)
    {
    }

    /// <summary>
    /// Gets the unique key for the content type
    /// </summary>
    [GraphQLDescription("Gets the unique key for the content type.")]
    public virtual Guid Key => PublishedContentType.Key;

    /// <summary>
    /// Gets the content type identifier
    /// </summary>
    [GraphQLDescription("Gets the content type identifier.")]
    public virtual int Id => PublishedContentType.Id;

    /// <summary>
    /// Gets the content type alias
    /// </summary>
    [GraphQLDescription("Gets the content type alias.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Alias is a name native to the Umbraco landscape and should therefore be easy to understand.")]
    public virtual string? Alias => PublishedContentType.Alias;

    /// <summary>
    /// Gets the content item type
    /// </summary>
    [GraphQLDescription("Gets the content item type.")]
    public virtual PublishedItemType ItemType => PublishedContentType.ItemType;

    /// <summary>
    /// Gets the aliases of the content types participating in the composition
    /// </summary>
    [GraphQLDescription("Gets the aliases of the content types participating in the composition.")]
    public virtual HashSet<string>? CompositionAliases => PublishedContentType.CompositionAliases;

    /// <summary>
    /// Gets the content variations of the content type
    /// </summary>
    [GraphQLDescription("Gets the content variations of the content type.")]
    public virtual Umbraco.Cms.Core.Models.ContentVariation Variations => PublishedContentType.Variations;

    /// <summary>
    /// Gets a value indicating whether this content type is for an element
    /// </summary>
    [GraphQLDescription("Gets a value indicating whether this content type is for an element.")]
    public virtual bool IsElement => PublishedContentType.IsElement;
}
