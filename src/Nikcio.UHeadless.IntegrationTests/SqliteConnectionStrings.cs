namespace Nikcio.UHeadless.IntegrationTests;

public static class SqliteConnectionStrings
{
    public static string ConnectionString(string testDatabaseSource)
    {
        return $"Data Source={testDatabaseSource};Cache=Shared;Foreign Keys=True;Pooling=True";
    }
}
