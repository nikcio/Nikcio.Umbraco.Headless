namespace Nikcio.UHeadless.Core.Maps;

/// <summary>
/// The base for maps
/// </summary>
public abstract class DictionaryMap
{
    /// <summary>
    /// Adds a mapping to a dictionary map
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="key"></param>
    /// <param name="map"></param>
    /// <returns>Whether the mapping has been added</returns>
    protected virtual bool AddMapping<TType>(string key, Dictionary<string, string> map) where TType : class
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(map);

        if (!map.ContainsKey(key))
        {
            lock (map)
            {
                if (!map.ContainsKey(key))
                {
                    string? assemblyQualifiedName = typeof(TType).AssemblyQualifiedName;
                    if (assemblyQualifiedName != null)
                    {
                        map.Add(key, assemblyQualifiedName);
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
