namespace Nikcio.UHeadless.IntegrationTests;

public static class SqliteConnectionStrings
{
    public static string ConnectionString(string testDatabaseName)
    {
        return $"Data Source=|DataDirectory|/{testDatabaseName};Cache=Shared;Foreign Keys=True;Pooling=True";
    }
}
