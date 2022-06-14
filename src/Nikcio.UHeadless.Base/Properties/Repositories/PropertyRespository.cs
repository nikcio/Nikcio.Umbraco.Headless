﻿using Nikcio.UHeadless.Base.Elements.Models;
using Nikcio.UHeadless.Base.Elements.Repositories;
using Nikcio.UHeadless.Base.Properties.Factories;
using Nikcio.UHeadless.Base.Properties.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.PublishedCache;

namespace Nikcio.UHeadless.Base.Properties.Repositories {
    /// <inheritdoc/>
    public class PropertyRespository<TProperty> : ElementRepository<IElement<TProperty>, TProperty>, IPropertyRespository<TProperty>
        where TProperty : IProperty {
        /// <summary>
        /// A factory for creating properties
        /// </summary>
        protected readonly IPropertyFactory<TProperty> propertyFactory;

        /// <inheritdoc/>
        public PropertyRespository(IPropertyFactory<TProperty> propertyFactory, IPublishedSnapshotAccessor publishedSnapshotAccessor, IUmbracoContextFactory umbracoContextFactory) : base(publishedSnapshotAccessor, umbracoContextFactory, propertyFactory) {
            this.propertyFactory = propertyFactory;
        }


        /// <inheritdoc/>
        public virtual IEnumerable<TProperty?> GetContentItemProperties(Func<IPublishedContentCache?, IPublishedContent?> fetch, string? culture) {
            var publishedCache = base.GetPublishedCache(publishedCache => publishedCache.Content);
            if (publishedCache is not null and IPublishedContentCache publishedContentCache) {
                var content = fetch(publishedContentCache);
                if (content != null) {
                    return GetProperties(content, culture);
                }
            }
            return Enumerable.Empty<TProperty>();
        }

        /// <inheritdoc/>
        public virtual IEnumerable<IEnumerable<TProperty?>?> GetContentItemsProperties(Func<IPublishedContentCache?, IEnumerable<IPublishedContent>?> fetch, string? culture) {
            var publishedCache = base.GetPublishedCache(publishedCache => publishedCache.Content);
            if (publishedCache is not null and IPublishedContentCache publishedContentCache) {
                var contentItems = fetch(publishedContentCache);
                if (contentItems != null) {
                    return contentItems.Select(content => GetProperties(content, culture));
                }
            }
            return Enumerable.Empty<IEnumerable<TProperty?>>();
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TProperty?> GetProperties(IPublishedContent content, string? culture) {
            return content.Properties.Select(IPublishedProperty => propertyFactory.GetProperty(IPublishedProperty, content, culture));
        }
    }
}