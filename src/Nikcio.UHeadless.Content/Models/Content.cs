using HotChocolate;
using Nikcio.UHeadless.Base.Elements.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Commands;

namespace Nikcio.UHeadless.Content.Models;

/// <summary>
/// A base for content
/// </summary>
/// <typeparam name="TProperty"></typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "This is to follow Umbraco naming and it's also the cause of the namespace.")]
public abstract class Content<TProperty> : Element<TProperty>, IContent
    where TProperty : IProperty
{
    /// <inheritdoc/>
    protected Content(CreateContent createContent, IPropertyFactory<TProperty> propertyFactory) : 
        base(
            createContent?.CreateElement ?? throw new ArgumentNullException(nameof(createContent)), 
            propertyFactory
        )
    {
    }

    /// <inheritdoc/>
    [GraphQLDescription("Redirect information for a content node")]
    public object? Redirect { get; set; }
}
