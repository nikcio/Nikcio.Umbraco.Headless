using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.ContentItems;

public partial class ContentItemBase
{
    /// <summary>
    /// A command to create a content item
    /// </summary>
    public class CreateCommand
    {
        /// <summary>
        /// The published content
        /// </summary>
        public required IPublishedContent? PublishedContent { get; init; }

        /// <summary>
        /// The culture
        /// </summary>
        public required string? Culture { get; init; }

        /// <summary>
        /// The segment
        /// </summary>
        public required string? Segment { get; init; }

        /// <summary>
        /// The fallback tactic
        /// </summary>
        public required Fallback? Fallback { get; init; }

        /// <summary>
        /// The resolver context
        /// </summary>
        /// <remarks>
        /// This can be fetched via dependency injection.
        /// </remarks>
        public required IResolverContext ResolverContext { get; init; }
    }
}
