using Nikcio.UHeadless.Base.Elements.Factories;
using Nikcio.UHeadless.Base.Elements.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace Nikcio.UHeadless.Base.Elements.Repositories;

/// <summary>
/// A repository for elements
/// </summary>
/// <typeparam name="TElement"></typeparam>
public abstract class ElementRepository<TElement>
    where TElement : IElement
{
    /// <summary>
    /// A factory for creating elements
    /// </summary>
    protected IElementFactory<TElement> elementFactory { get; }

    /// <inheritdoc/>
    protected ElementRepository(IUmbracoContextFactory umbracoContextFactory, IElementFactory<TElement> elementFactory)
    {
        ArgumentNullException.ThrowIfNull(umbracoContextFactory);

        umbracoContextFactory.EnsureUmbracoContext();
        this.elementFactory = elementFactory;
    }

    /// <summary>
    /// Gets an element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    protected virtual TElement? GetElement(IPublishedContent? element, string? culture, string? segment, Fallback? fallback)
    {
        if (element == null)
        {
            return default;
        }

        return GetConvertedElement(element, culture, segment, fallback);
    }

    /// <summary>
    /// Gets a list of elements
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    protected virtual IEnumerable<TElement?> GetElementList(IEnumerable<IPublishedContent>? elements, string? culture, string? segment, Fallback? fallback)
    {
        if (elements == null)
        {
            return Enumerable.Empty<TElement>();
        }

        return elements.Select(element => GetConvertedElement(element, culture, segment, fallback));
    }

    /// <summary>
    /// Get the converted element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="culture"></param>
    /// <param name="segment"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    protected virtual TElement? GetConvertedElement(IPublishedContent? element, string? culture, string? segment, Fallback? fallback)
    {
        return elementFactory.CreateElement(element, culture, segment, fallback);
    }
}
