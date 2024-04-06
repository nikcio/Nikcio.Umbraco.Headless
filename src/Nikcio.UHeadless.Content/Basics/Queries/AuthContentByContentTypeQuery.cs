using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Content.Basics.Models;
using Nikcio.UHeadless.Content.Queries;
using Nikcio.UHeadless.Content.Repositories;
using Nikcio.UHeadless.Core.GraphQL.Queries;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Content.Basics.Queries;

/// <summary>
/// The default query implementation of the ContentByContentType query
/// </summary>
[ExtendObjectType(typeof(Query))]
public class AuthContentByContentTypeQuery : ContentByContentTypeQuery<BasicContent>
{
    /// <inheritdoc />
    [Authorize]
    public override IEnumerable<BasicContent?> ContentByContentType(
        [Service] IContentRepository<BasicContent> contentRepository,
        [Service] IVariationContextAccessor variationContextAccessor,
        [GraphQLDescription("The contentType to fetch.")] string contentType,
        [GraphQLDescription("The culture.")] string? culture = null,
        [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false,
        [GraphQLDescription("The property variation segment")] string? segment = null,
        [GraphQLDescription("The property value fallback strategy")] IEnumerable<PropertyFallback>? fallback = null)
    {
        return base.ContentByContentType(contentRepository, variationContextAccessor, contentType, culture, preview, segment, fallback);
    }
}
