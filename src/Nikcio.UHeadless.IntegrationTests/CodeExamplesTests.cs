namespace Nikcio.UHeadless.IntegrationTests;

[Collection("IntegrationTests")]
public partial class CodeExamplesTests : IClassFixture<CodeExamplesApplicationFactory>
{
    private readonly CodeExamplesApplicationFactory _factory;

    public CodeExamplesTests(CodeExamplesApplicationFactory factory)
    {
        _factory = factory;
    }
}
