namespace Nikcio.UHeadless.IntegrationTests.Defaults;

public partial class ApiTests
{
    private const string _schemaSnapshotPath = $"{SnapshotConstants.BasePath}/Schema";

    [Fact]
    public async Task Can_Get_Schema_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_schemaSnapshotPath}");
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/graphql?sdl").ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

        string snapshotName = $"Schema";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
