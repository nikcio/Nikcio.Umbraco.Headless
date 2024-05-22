namespace Nikcio.UHeadless.IntegrationTests;

[Collection(nameof(ApplicationFactoryBase))]
public partial class ApiTests : IClassFixture<UnAuthenticatedApplicationFactory>
{
    private readonly UnAuthenticatedApplicationFactory _factory;

    public ApiTests(UnAuthenticatedApplicationFactory factory)
    {
        _factory = factory;
    }
}
