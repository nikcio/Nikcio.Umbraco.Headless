using HotChocolate;
using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Models;

namespace Nikcio.UHeadless.Content.Basics.Models;

/// <summary>
/// Represents a content redirect
/// </summary>
[GraphQLDescription("Represents a content redirect")]
public class BasicContentRedirect : ContentRedirect
{
    /// <inheritdoc/>
    public BasicContentRedirect(CreateContentRedirect createContentRedirect) : base(createContentRedirect)
    {
        ArgumentNullException.ThrowIfNull(createContentRedirect);

        RedirectUrl = createContentRedirect.RedirectUrl;
        IsPermanent = createContentRedirect.IsPermanent;
    }

    /// <summary>
    /// The url to redirect to
    /// </summary>
    [GraphQLDescription("The url to redirect to")]
    public virtual string RedirectUrl { get; set; }

    /// <summary>
    /// Is the redirect permanent
    /// </summary>
    [GraphQLDescription("Is the redirect permanent")]
    public virtual bool IsPermanent { get; set; }
}
