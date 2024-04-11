using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Media;

public partial class MemberItem
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
        /// The resolver context
        /// </summary>
        /// <remarks>
        /// This can be fetched via dependency injection.
        /// </remarks>
        public required IResolverContext ResolverContext { get; init; }
    }
}
