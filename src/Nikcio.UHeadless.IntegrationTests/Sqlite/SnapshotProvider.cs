using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public class SnapshotProvider
{
    private readonly string _snapshotFolder;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public SnapshotProvider(string snapshotFolder)
    {
        // We need to go up 3 levels to get to the root of the project instead of bin/Debug/netX.Z
        _snapshotFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..", snapshotFolder);
    }

    public async Task AssertIsSnapshotEqualAsync(string snapshotName, string content)
    {
        var jsonContent = await GetContentAsJsonAsync(content);
        var snapshot = GetSnapshotAsync(snapshotName, content).Result;
        Assert.Equal(snapshot, jsonContent);
    }

    private async Task<string> GetSnapshotAsync(string snapshotName, string content)
    {
        var snapshotPath = Path.Combine(_snapshotFolder, snapshotName);
        if (File.Exists(snapshotPath))
        {
            return await File.ReadAllTextAsync(snapshotPath);
        } else
        {
            if (!Directory.Exists(_snapshotFolder))
            {
                Directory.CreateDirectory(_snapshotFolder);
            }

            var contentToSave = await GetContentAsJsonAsync(content);
            await File.WriteAllTextAsync(snapshotPath, contentToSave);

            throw new Exception($"Snapshot '{snapshotName}' not found. Created a new snapshot at {snapshotPath}");
        }
    }

    private async Task<string> GetContentAsJsonAsync(string content)
    {
        // We want to write the JSON in a human readable format for easier debugging
        await using var jsonStream = new MemoryStream();
        var jsonObject = JsonSerializer.Deserialize<JsonNode>(content);
        if (jsonObject != null)
        {
            await JsonSerializer.SerializeAsync(jsonStream, jsonObject, _options);
            var json = Encoding.UTF8.GetString(jsonStream.ToArray());
            return Encoding.UTF8.GetString(jsonStream.ToArray());
        } else
        {
            // Anything that is not JSON, we just write as is - This could be an error message or something else
            return content;
        }
    }
}