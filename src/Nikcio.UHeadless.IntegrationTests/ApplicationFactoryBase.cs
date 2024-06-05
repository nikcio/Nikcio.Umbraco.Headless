using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Nikcio.UHeadless.IntegrationTests;

public abstract class ApplicationFactoryBase<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public abstract UHeadlessSetup UHeadlessSetup { get; }

    public abstract string TestDatabaseName { get; }

    public override async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await base.DisposeAsync().ConfigureAwait(true);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:umbracoDbDSN"] = SqliteConnectionStrings.ConnectionString(TestDatabaseName),
                ["ConnectionStrings:umbracoDbDSN_ProviderName"] = "Microsoft.Data.Sqlite",
                [nameof(UHeadlessSetup)] = UHeadlessSetup.GetType().FullName,
            });
        });

        builder.UseEnvironment("Testing");

        return base.CreateHost(builder);
    }
}

public abstract class UHeadlessSetup
{
    public abstract Action<UHeadlessOptions> GetSetup();
}
