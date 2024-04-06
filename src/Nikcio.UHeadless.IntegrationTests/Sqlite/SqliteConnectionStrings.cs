namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public static class SqliteConnectionStrings
{
    public static string ConnectionString()
    {
        return $"Data Source=|DataDirectory|/Sharable-Test;Cache=Shared;Foreign Keys=True;Pooling=True";
    }
}
