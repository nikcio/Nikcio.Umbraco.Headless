using System;
using HotChocolate;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Base.Basics.Models;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Content.Basics.Models;
using Nikcio.UHeadless.Content.Commands;
using Nikcio.UHeadless.Content.Factories;
using Nikcio.UHeadless.ContentTypes.Basics.Models;
using Nikcio.UHeadless.ContentTypes.Factories;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Examples.Docs.Content.PublicAccessExample;

[GraphQLDescription("Represents an extended content item.")]
public class CustomBasicContent : BasicContent<BasicProperty, BasicContentType, BasicContentRedirect, CustomBasicContent>
{
    [GraphQLDescription("Gets the restrict public access settings of the content item.")]
    public PermissionsModel? Permissions { get; set; }

    public CustomBasicContent(CreateContent createContent, IPropertyFactory<BasicProperty> propertyFactory, IContentTypeFactory<BasicContentType> contentTypeFactory, IContentFactory<CustomBasicContent> contentFactory, IVariationContextAccessor variationContextAccessor, IPublicAccessService publicAccessService, IContentService contentService, IUmbracoContextAccessor context, ILogger<CustomBasicContent> logger) : base(createContent, propertyFactory, contentTypeFactory, contentFactory, variationContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(createContent);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(publicAccessService);
        ArgumentNullException.ThrowIfNull(contentService);

        if (createContent.Content == null)
        {
            logger.LogWarning("Content is null");
            return;
        }

        IContent? content = contentService.GetById(createContent.Content.Id);

        if (content == null)
        {
            logger.LogWarning("Content from content service is null. Id: {ContentId}", createContent.Content.Id);
            return;
        }

        PublicAccessEntry? entry = publicAccessService.GetEntryForContent(content);

        if (entry != null)
        {
            IUmbracoContext cache = context.GetRequiredUmbracoContext();

            if (cache.Content == null)
            {
                logger.LogWarning("Content cache is null on Umbraco context");
                return;
            }

            IPublishedContent? loginContent = cache.Content.GetById(entry.LoginNodeId);
            IPublishedContent? noAccessContent = cache.Content.GetById(entry.NoAccessNodeId);

            Permissions = new PermissionsModel
            {
                UrlLogin = loginContent?.Url(),
                UrlNoAccess = noAccessContent?.Url()
            };

            foreach (PublicAccessRule rule in entry.Rules)
            {
                Permissions.AccessRules.Add(new AccessRuleModel(rule.RuleType, rule.RuleValue));
            }
        }
    }
}
