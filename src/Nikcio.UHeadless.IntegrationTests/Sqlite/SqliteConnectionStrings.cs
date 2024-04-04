namespace Nikcio.UHeadless.IntegrationTests.Sqlite;

public static class SqliteConnectionStrings
{
    public static string ConnectionString()
    {
        return $"Data Source=Sharable-{Guid.NewGuid()};Mode=Memory;Cache=Shared;Foreign Keys=True;Pooling=True";
    }
}
