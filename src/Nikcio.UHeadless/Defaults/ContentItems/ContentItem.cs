using HotChocolate.Resolvers;
using Nikcio.UHeadless.ContentItems;
using Nikcio.UHeadless.Properties;
using Nikcio.UHeadless.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.Defaults.ContentItems;

public partial class ContentItem : ContentItemBase
{
    protected IVariationContextAccessor VariationContextAccessor { get; }

    protected IDependencyReflectorFactory DependencyReflectorFactory { get; }

    public ContentItem(CreateCommand command) : base(command)
    {
        ArgumentNullException.ThrowIfNull(command);

        VariationContextAccessor = command.ResolverContext.Service<IVariationContextAccessor>();
        DependencyReflectorFactory = command.ResolverContext.Service<IDependencyReflectorFactory>();

        StatusCode = command.StatusCode;
        Redirect = command.Redirect == null ? null : new RedirectInfo()
        {
            IsPermanent = command.Redirect.IsPermanent,
            RedirectUrl = command.Redirect.RedirectUrl
        };
    }

    /// <summary>
    /// Gets the url segment of the content item
    /// </summary>
    [GraphQLDescription("Gets the url segment of the content item.")]
    public string? UrlSegment(IResolverContext resolverContext)
    {
        return PublishedContent?.UrlSegment(VariationContextAccessor, resolverContext.Culture());
    }

    /// <summary>
    /// Gets the url of a content item
    /// </summary>
    [GraphQLDescription("Gets the url of a content item.")]
    public string? Url(IResolverContext resolverContext, UrlMode urlMode)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        return PublishedContent?.Url(resolverContext.Service<IPublishedUrlProvider>(), resolverContext.Culture(), urlMode);
    }

    /// <summary>
    /// Gets the name of a content item
    /// </summary>
    [GraphQLDescription("Gets the name of a content item.")]
    public string? Name(IResolverContext resolverContext)
    {
        return PublishedContent?.Name(VariationContextAccessor, resolverContext.Culture());
    }

    /// <summary>
    /// Gets the id of a content item
    /// </summary>
    [GraphQLDescription("Gets the id of a content item.")]
    public int? Id => PublishedContent?.Id;

    /// <summary>
    /// Gets the key of a content item
    /// </summary>
    [GraphQLDescription("Gets the key of a content item.")]
    public Guid? Key => PublishedContent?.Key;

    /// <summary>
    /// Gets the identifier of the template to use to render the content item
    /// </summary>
    [GraphQLDescription("Gets the identifier of the template to use to render the content item.")]
    public int? TemplateId => PublishedContent?.TemplateId;

    /// <summary>
    /// Gets the date the content item was last updated
    /// </summary>
    [GraphQLDescription("Gets the date the content item was last updated.")]
    public DateTime? UpdateDate => PublishedContent?.UpdateDate;

    /// <summary>
    /// Gets the parent of the content item
    /// </summary>
    [GraphQLDescription("Gets the parent of the content item.")]
    public ContentItem? Parent(IResolverContext resolverContext)
    {
        return PublishedContent?.Parent != null ? CreateContentItem<ContentItem>(new CreateCommand()
        {
            PublishedContent = PublishedContent.Parent,
            ResolverContext = resolverContext,
            Redirect = null,
            StatusCode = 200,
        }, DependencyReflectorFactory) : default;
    }

    /// <summary>
    /// Gets the properties of the content item
    /// </summary>
    [GraphQLDescription("Gets the properties of the content item.")]
    public TypedProperties Properties(IResolverContext resolverContext)
    {
        resolverContext.SetScopedState(ContextDataKeys.PublishedContent, PublishedContent);
        return new TypedProperties();
    }

    public int StatusCode { get; }

    /// <summary>
    /// Gets the redirect information for the content item
    /// </summary>
    [GraphQLDescription("Gets the redirect information for the content item.")]
    public RedirectInfo? Redirect { get; }

    public class RedirectInfo
    {
        public required string? RedirectUrl { get; init; }

        public required bool IsPermanent { get; init; }
    }
}
