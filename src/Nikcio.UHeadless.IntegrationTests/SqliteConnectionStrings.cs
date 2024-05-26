namespace Nikcio.UHeadless.IntegrationTests;

public static class SqliteConnectionStrings
{
    public static string ConnectionString()
    {
        return $"Data Source=|DataDirectory|/Default-Tests.sqlite;Cache=Shared;Foreign Keys=True;Pooling=True";
    }
}
