namespace Nikcio.UHeadless.IntegrationTests;

[Collection("IntegrationTests")]
public partial class ApiAuthTests : IClassFixture<AuthenticatedApplicationFactory>
{
    private readonly AuthenticatedApplicationFactory _factory;

    public ApiAuthTests(AuthenticatedApplicationFactory factory)
    {
        _factory = factory;
    }
}
