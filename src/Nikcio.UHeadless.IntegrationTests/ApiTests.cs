namespace Nikcio.UHeadless.IntegrationTests.Defaults;

[Collection(nameof(ApplicationFactoryBase))]
public partial class ApiTests : IClassFixture<UnAuthenticatedApplicationFactory>
{
    private readonly UnAuthenticatedApplicationFactory _factory;

    public ApiTests(UnAuthenticatedApplicationFactory factory)
    {
        _factory = factory;
    }
}
