namespace Nikcio.UHeadless.IntegrationTests;

public partial class ApiTests
{
    private const string _schemaSnapshotPath = $"{SnapshotConstants.BasePath}/Schema";

    [Fact]
    public async Task Can_Get_Schema_Async()
    {
        var snapshotProvider = new SnapshotProvider($"{_schemaSnapshotPath}");
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/graphql?sdl", TestContext.Current.CancellationToken).ConfigureAwait(true);

        string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).ConfigureAwait(true);

        string snapshotName = $"Schema.snap";

        await snapshotProvider.AssertIsSnapshotEqualAsync(snapshotName, responseContent).ConfigureAwait(true);
        Assert.True(response.IsSuccessStatusCode);
    }
}
