namespace Nikcio.UHeadless;

/// <summary>
/// Extensions for strings
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Capitalizes the first letter of a string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string FirstCharToUpper(this string input)
    {
        return input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpperInvariant(), input.AsSpan(1))
        };
    }
}
