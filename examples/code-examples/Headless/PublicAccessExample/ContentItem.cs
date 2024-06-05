using HotChocolate;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using HotChocolate.Resolvers;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Nikcio.UHeadless;

namespace Code.Examples.Headless.PublicAccessExample;

/// <summary>
/// This example demonstrates how to create a custom content item that includes the public access settings from Umbraco.
/// with this information you can block access to content based on the user using the site.
/// </summary>
/// <remarks>
/// This example uses the default <see cref="Nikcio.UHeadless.Defaults.ContentItems.ContentItem"/> class from UHeadless
/// but could also be implemented using the <see cref="Nikcio.UHeadless.ContentItems.ContentItemBase"/> class.
/// </remarks>
[GraphQLName("PublishAccessExampleContentItem")]
public class ContentItem : Nikcio.UHeadless.Defaults.ContentItems.ContentItem
{
    public ContentItem(CreateCommand command) : base(command)
    {
    }

    [GraphQLDescription("Gets the restrict public access settings of the content item.")]
    public PermissionsModel? Permissions(IResolverContext resolverContext)
    {
        ArgumentNullException.ThrowIfNull(resolverContext);

        ILogger<ContentItem> logger = resolverContext.Service<ILogger<ContentItem>>();
        IContentService contentService = resolverContext.Service<IContentService>();
        IPublicAccessService publicAccessService = resolverContext.Service<IPublicAccessService>();
        IContentItemRepository<ContentItem> contentItemRepository = resolverContext.Service<IContentItemRepository<ContentItem>>();

        if (PublishedContent == null)
        {
            logger.LogWarning("Content is null");
            return null;
        }

        IContent? content = contentService.GetById(PublishedContent.Id);

        if (content == null)
        {
            logger.LogWarning("Content from content service is null. Id: {ContentId}", PublishedContent.Id);
            return null;
        }

        PublicAccessEntry? entry = publicAccessService.GetEntryForContent(content);

        if (entry == null)
        {
            logger.LogWarning("Public access entry is null. ContentId: {ContentId}", PublishedContent.Id);
            return null;
        }

        IPublishedContentCache? contentCache = contentItemRepository.GetCache();

        if (contentCache == null)
        {
            throw new InvalidOperationException("The content cache is not available");
        }

        IPublishedContent? loginContent = contentCache.GetById(entry.LoginNodeId);
        IPublishedContent? noAccessContent = contentCache.GetById(entry.NoAccessNodeId);

        var permissions = new PermissionsModel
        {
            UrlLogin = loginContent?.Url(resolverContext.Service<IPublishedUrlProvider>(), resolverContext.Culture(), UrlMode.Absolute),
            UrlNoAccess = noAccessContent?.Url(resolverContext.Service<IPublishedUrlProvider>(), resolverContext.Culture(), UrlMode.Absolute)
        };

        foreach (PublicAccessRule rule in entry.Rules)
        {
            permissions.AccessRules.Add(new AccessRuleModel(rule.RuleType, rule.RuleValue));
        }

        return permissions;
    }
}
