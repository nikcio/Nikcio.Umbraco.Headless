using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public partial class SnapshotProvider
{
    private readonly string _snapshotFolder;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public SnapshotProvider(string snapshotFolder)
    {
        // We need to go up 3 levels to get to the root of the project instead of bin/Debug/netX.Z
        _snapshotFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../..", snapshotFolder);
    }

    public async Task AssertIsSnapshotEqualAsync(string snapshotName, string content)
    {
        var jsonContent = await GetContentAsJsonAsync(content);
        var snapshot = await GetSnapshotAsync(snapshotName, content);
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
            return FormatDateTimesAsUTCInJson(Encoding.UTF8.GetString(jsonStream.ToArray()));
        } else
        {
            // Anything that is not JSON, we just write as is - This could be an error message or something else
            return content;
        }
    }

    /// <summary>
    /// This makes sure that all date times in the JSON are formatted as UTC this will make the snapshots more stable as the time zone will not be a factor
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    private string FormatDateTimesAsUTCInJson(string json)
    {
        return DateTimeRegex().Replace(json, "$1Z");
    }

    [GeneratedRegex("""(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3,})((\\u002B|\+)\d{2}:\d{2})""")]
    private static partial Regex DateTimeRegex();
}