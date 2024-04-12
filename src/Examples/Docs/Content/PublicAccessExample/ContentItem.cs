using HotChocolate;
using Nikcio.UHeadless.ContentItems;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace Examples.Docs.Content.PublicAccessExample;

[GraphQLDescription("Represents an extended content item.")]
public class ContentItem : ContentItemBase
{
    private readonly IPublicAccessService _publicAccessService;
    private readonly IContentService _contentService;
    private readonly IUmbracoContextAccessor _umbracoContext;
    private readonly ILogger<ContentItem> _logger;

    [GraphQLDescription("Gets the restrict public access settings of the content item.")]
    public PermissionsModel? Permissions()
    {

        if (PublishedContent == null)
        {
            _logger.LogWarning("Content is null");
            return null;
        }

        IContent? content = _contentService.GetById(PublishedContent.Id);

        if (content == null)
        {
            _logger.LogWarning("Content from content service is null. Id: {ContentId}", PublishedContent.Id);
            return null;
        }

        PublicAccessEntry? entry = _publicAccessService.GetEntryForContent(content);

        if (entry == null)
        {
            _logger.LogWarning("Public access entry is null. ContentId: {ContentId}", PublishedContent.Id);
            return null;
        }

        IUmbracoContext cache = _umbracoContext.GetRequiredUmbracoContext();

        if (cache.Content == null)
        {
            _logger.LogWarning("Content cache is null on Umbraco context");
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

    public ContentItem(
        CreateCommand command,
        IPublicAccessService publicAccessService,
        IContentService contentService,
        IUmbracoContextAccessor umbracoContext,
        ILogger<ContentItem> logger) : base(command)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(publicAccessService);
        ArgumentNullException.ThrowIfNull(contentService);

        _publicAccessService = publicAccessService;
        _contentService = contentService;
        _umbracoContext = umbracoContext;
        _logger = logger;
    }
}
