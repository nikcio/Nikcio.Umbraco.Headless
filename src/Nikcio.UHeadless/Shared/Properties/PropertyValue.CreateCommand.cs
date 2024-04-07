using HotChocolate.Resolvers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Shared.Properties;

public partial class PropertyValue
{
    public class CreateCommand
    {
        /// <summary>
        /// The <see cref="IPublishedProperty"/>
        /// </summary>
        public required IPublishedProperty PublishedProperty { get; init; }

        /// <summary>
        /// The published value fallback
        /// </summary>
        public required IPublishedValueFallback PublishedValueFallback { get; init; }

        /// <summary>
        /// The resolver context
        /// </summary>
        /// <remarks>
        /// This needs to be the same as the one used on the model where the property value is used.
        /// </remarks>
        public required IResolverContext ResolverContext { get; init; }
    }
}
