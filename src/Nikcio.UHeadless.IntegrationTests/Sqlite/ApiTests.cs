namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class ApiTests : IClassFixture<ApplicactionFactory>
{
    private readonly ApplicactionFactory _factory;

    public ApiTests(ApplicactionFactory factory)
    {
        _factory = factory;
    }
}
