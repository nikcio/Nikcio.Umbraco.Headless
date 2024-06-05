namespace Nikcio.UHeadless.IntegrationTests;

[Collection("IntegrationTests")]
public partial class ApiTests : IClassFixture<UnAuthenticatedApplicationFactory>
{
    private readonly UnAuthenticatedApplicationFactory _factory;

    public ApiTests(UnAuthenticatedApplicationFactory factory)
    {
        _factory = factory;
    }
}
