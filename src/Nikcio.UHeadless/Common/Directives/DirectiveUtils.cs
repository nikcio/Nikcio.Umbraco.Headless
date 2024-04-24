using System.Globalization;

namespace Nikcio.UHeadless.Common.Directives;

internal static class DirectiveUtils
{
    public static string ArgumentName(string argumentName)
    {
        return string.Concat(argumentName.First().ToString().ToLower(CultureInfo.CurrentCulture), argumentName.AsSpan(1));
    }
}
