using HotChocolate;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using HotChocolate.Resolvers;

namespace Code.Examples.Headless.PublicAccessExample;

/// <summary>
/// This example demonstrates how to create a custom content item that includes the public access settings from Umbraco.
/// with this information you can block access to content based on the user using the site.
/// </summary>
/// <remarks>
/// This example uses the default <see cref="Nikcio.UHeadless.Defaults.ContentItems.ContentItem"/> class from UHeadless
/// but could also be implemented using the <see cref="Nikcio.UHeadless.ContentItems.ContentItemBase"/> class.
/// </remarks>
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
        IUmbracoContextAccessor umbracoContext = resolverContext.Service<IUmbracoContextAccessor>();

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

        IUmbracoContext cache = umbracoContext.GetRequiredUmbracoContext();

        if (cache.Content == null)
        {
            logger.LogWarning("Content cache is null on Umbraco context");
            return null;
        }

        IPublishedContent? loginContent = cache.Content.GetById(entry.LoginNodeId);
        IPublishedContent? noAccessContent = cache.Content.GetById(entry.NoAccessNodeId);

        var permissions = new PermissionsModel
        {
            UrlLogin = loginContent?.Url(),
            UrlNoAccess = noAccessContent?.Url()
        };

        foreach (PublicAccessRule rule in entry.Rules)
        {
            permissions.AccessRules.Add(new AccessRuleModel(rule.RuleType, rule.RuleValue));
        }

        return permissions;
    }
}
