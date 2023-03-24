using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Nikcio.UHeadless.IntegrationTests.TestProject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nikcio.UHeadless.IntegrationTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class IntegrationTestBase
{
   protected IntegrationTestFactory Factory { get; } = new IntegrationTestFactory();

    protected AsyncServiceScope Scope { get; private set; }
    protected IServiceProvider ServiceProvider => Scope.ServiceProvider;

    [SetUp]
    public virtual void Setup()
    {
        Scope = Factory.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
    }

    [TearDown]
    public virtual void TearDown()
    {
        Scope.Dispose();
        Factory.Dispose();
    }

    protected virtual HttpClient Client => Factory.CreateClient();

    protected virtual TType? GetService<TType>() => ServiceProvider.GetService<TType>();

    protected virtual TType GetRequiredService<TType>() => ServiceProvider.GetService<TType>() ?? throw new InvalidOperationException("Unable to get service.");
}

public class IntegrationTestFactory : WebApplicationFactory<Program>
{
    private string DataSource = Guid.NewGuid().ToString();
    private string _inMemoryConnectionString => $"Data Source={DataSource};Mode=Memory;Cache=Shared";
    private readonly SqliteConnection _databaseConnection;
    public IntegrationTestFactory()
    {
        // Shared in-memory databases get destroyed when the last connection is closed.
        // Keeping a connection open while this web application is used, ensures that the database does not get destroyed in the middle of a test.
        _databaseConnection = new SqliteConnection(_inMemoryConnectionString);
        _databaseConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        builder.ConfigureAppConfiguration(conf =>
        {
            conf.AddInMemoryCollection(new KeyValuePair<string, string?>[]
            {
                new KeyValuePair<string, string?>("ConnectionStrings:umbracoDbDSN", _inMemoryConnectionString),
                new KeyValuePair<string, string?>("ConnectionStrings:umbracoDbDSN_ProviderName", "Microsoft.Data.Sqlite")
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        // When this application factory is disposed, close the connection to the in-memory database
        // This will destroy the in-memory database
        _databaseConnection.Close();
        _databaseConnection.Dispose();
    }
}
