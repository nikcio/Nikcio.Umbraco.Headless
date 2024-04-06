using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public class ApplicactionFactory : WebApplicationFactory<Program>//, IAsyncLifetime
{
    //private readonly SqliteConnection _sqliteConnection = new(SqliteConnectionStrings.ConnectionString());

    //public async Task InitializeAsync()
    //{
    //    await _sqliteConnection.OpenAsync().ConfigureAwait(true);
    //}

    //Task IAsyncLifetime.DisposeAsync()
    //{
    //    return _sqliteConnection.CloseAsync();
    //}

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:umbracoDbDSN"] = SqliteConnectionStrings.ConnectionString(),//_sqliteConnection.ConnectionString,
                ["ConnectionStrings:umbracoDbDSN_ProviderName"] = "Microsoft.Data.Sqlite"
            });
        });

    }

    public override async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await base.DisposeAsync().ConfigureAwait(true);
        //await _sqliteConnection.DisposeAsync().ConfigureAwait(true);
    }
}
