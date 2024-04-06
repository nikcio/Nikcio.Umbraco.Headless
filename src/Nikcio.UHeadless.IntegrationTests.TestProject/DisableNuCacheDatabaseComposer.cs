using Umbraco.Cms.Core.Composing;

namespace Nikcio.UHeadless.IntegrationTests.TestProject;

public class DisableNuCacheDatabaseComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var settings = new Umbraco.Cms.Infrastructure.PublishedCache.PublishedSnapshotServiceOptions
        {
            IgnoreLocalDb = true
        };
        builder.Services.AddSingleton(settings);
    }
}
