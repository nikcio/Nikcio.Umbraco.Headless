﻿using Nikcio.UHeadless.Base.Elements.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Commands;

namespace Nikcio.UHeadless.Content.Models;

/// <summary>
/// A base for content
/// </summary>
public abstract class Content : Element, IContent
{
    /// <inheritdoc/>
    protected Content(CreateContent createContent) : base(createContent.CreateElement)
    {
    }

    /// <inheritdoc/>
    public virtual object? Redirect { get; set; }
}
