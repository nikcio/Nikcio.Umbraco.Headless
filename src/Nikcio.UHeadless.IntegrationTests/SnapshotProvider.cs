using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Nikcio.UHeadless.IntegrationTests;

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
        string snapshotErrorName = $"{snapshotName}.error";
        string snapshotErrorPath = Path.Combine(_snapshotFolder, snapshotErrorName);

        string jsonContent = await GetContentAsJsonAsync(content).ConfigureAwait(true);
        string snapshot = await GetSnapshotAsync(snapshotName, content).ConfigureAwait(true);

        await CreateSnapshotAsync(jsonContent, snapshotErrorPath).ConfigureAwait(true);

        Assert.Equal(snapshot, jsonContent);

        // If the assertion passes we can delete the error snapshot
        File.Delete(snapshotErrorPath);
    }

    private async Task<string> GetSnapshotAsync(string snapshotName, string content)
    {
        string snapshotPath = Path.Combine(_snapshotFolder, snapshotName);
        if (File.Exists(snapshotPath))
        {
            return await File.ReadAllTextAsync(snapshotPath).ConfigureAwait(true);
        }
        else
        {
            await CreateSnapshotAsync(content, snapshotPath).ConfigureAwait(true);

            throw new SnapshotNotFoundException($"Snapshot '{snapshotName}' not found. Created a new snapshot at {snapshotPath}");
        }
    }

    private async Task CreateSnapshotAsync(string content, string snapshotPath)
    {
        if (!Directory.Exists(_snapshotFolder))
        {
            Directory.CreateDirectory(_snapshotFolder);
        }

        string contentToSave = await GetContentAsJsonAsync(content).ConfigureAwait(true);
        await File.WriteAllTextAsync(snapshotPath, contentToSave).ConfigureAwait(true);
    }

    private async Task<string> GetContentAsJsonAsync(string content)
    {
        // We want to write the JSON in a human readable format for easier debugging
        JsonNode? jsonObject = GetJsonObject(content);
        if (jsonObject != null)
        {
            var jsonStream = new MemoryStream();
            await using (jsonStream.ConfigureAwait(true))
            {
                await JsonSerializer.SerializeAsync(jsonStream, jsonObject, _options).ConfigureAwait(true);
                return FormatDateTimesAsUTCInJson(Encoding.UTF8.GetString(jsonStream.ToArray()));
            }

        }
        else
        {
            // Anything that is not JSON, we just write as is - This could be an error message or something else
            return content;
        }
    }

    private static JsonNode? GetJsonObject(string content)
    {
        JsonNode? jsonObject;

        // We want to catch any exceptions that might occur when trying to parse the content as JSON so we compare the response rather than if it is valid JSON
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            jsonObject = JsonSerializer.Deserialize<JsonNode>(content);
        }
        catch
        {
            jsonObject = null;
        }
#pragma warning restore CA1031 // Do not catch general exception types
        return jsonObject;
    }

    /// <summary>
    /// This makes sure that all date times in the JSON are formatted as UTC this will make the snapshots more stable as the time zone will not be a factor
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    private static string FormatDateTimesAsUTCInJson(string json)
    {
        return DateTimeRegex().Replace(json, "$1Z");
    }

    [GeneratedRegex("""(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3,})((\\u002B|\+)\d{2}:\d{2})""")]
    private static partial Regex DateTimeRegex();
}

public class SnapshotNotFoundException : Exception
{
    public SnapshotNotFoundException(string message) : base(message)
    {
    }

    public SnapshotNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public SnapshotNotFoundException()
    {
    }
}
