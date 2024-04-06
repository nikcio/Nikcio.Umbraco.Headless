using Examine.Lucene;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Infrastructure.Examine;

namespace Nikcio.UHeadless.IntegrationTests.TestProject;

/// <summary>
///     Configures the index options to construct the Examine indexes
/// </summary>
public sealed class ConfigureExamineIndexes : IConfigureNamedOptions<LuceneDirectoryIndexOptions>
{
    public void Configure(string? name, LuceneDirectoryIndexOptions options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        options.DirectoryFactory = new LuceneRAMDirectoryFactory();
    }

    public void Configure(LuceneDirectoryIndexOptions options)
    {
        throw new NotImplementedException("This is never called and is just part of the interface");
    }
}
