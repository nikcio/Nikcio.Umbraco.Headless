using Nikcio.UHeadless.Common.Properties;

namespace Nikcio.UHeadless.Properties;

/// <summary>
/// A map of all property types
/// </summary>
public interface IPropertyMap
{
    /// <summary>
    /// Adds a mapping of a type to a content type alias combined with a property type alias.
    /// </summary>
    /// <typeparam name="TType">The type that should be used for this property</typeparam>
    /// <param name="contentTypeAlias"></param>
    /// <param name="propertyTypeAlias"></param>
    /// <example>
    /// ContentTypeAlias: MyDocType
    /// PropertyTypeAlias: MyProperty
    /// </example>
    /// <remarks>This takes precedence over editor mappings</remarks>
    void AddAliasMapping<TType>(string contentTypeAlias, string propertyTypeAlias) where TType : PropertyValue;

    /// <summary>
    /// Adds a mapping of a type to a editor alias.
    /// </summary>
    /// <typeparam name="TType">The type that should be used for this property</typeparam>
    /// <param name="editorName"></param>
    void AddEditorMapping<TType>(string editorName) where TType : PropertyValue;

    /// <summary>
    /// Removes a alias mapping
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <param name="propertyTypeAlias"></param>
    /// <example>
    /// ContentTypeAlias: MyDocType
    /// PropertyTypeAlias: MyProperty
    /// </example>
    void RemoveAliasMapping(string contentTypeAlias, string propertyTypeAlias);

    /// <summary>
    /// Removes a editor mapping
    /// </summary>
    /// <param name="editorName"></param>
    void RemoveEditorMapping(string editorName);

    /// <summary>
    /// Checks if a alias is already in the map
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <param name="propertyTypeAlias"></param>
    /// <returns></returns>
    bool ContainsAlias(string contentTypeAlias, string propertyTypeAlias);

    /// <summary>
    /// Checks if a editor alias is already in the map
    /// </summary>
    /// <param name="editorName"></param>
    /// <returns></returns>
    bool ContainsEditor(string editorName);

    /// <summary>
    /// Gets a alias value
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <param name="propertyTypeAlias"></param>
    /// <returns>The types AssemblyQualifiedName</returns>
    string GetAliasValue(string contentTypeAlias, string propertyTypeAlias);

    /// <summary>
    /// Gets all types used in the property map
    /// </summary>
    /// <returns></returns>
    HashSet<Type> GetAllTypes();

    /// <summary>
    /// Get a editor value
    /// </summary>
    /// <param name="editorName"></param>
    /// <returns>he types AssemblyQualifiedName</returns>
    string GetEditorValue(string editorName);

    /// <summary>
    /// Gets the key for an alias mapping added with <see cref="AddAliasMapping{TType}(string, string)"/>
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <param name="propertyTypeAlias"></param>
    /// <returns></returns>
    string GetAliasMappingKey(string contentTypeAlias, string propertyTypeAlias);

    /// <summary>
    /// Gets the key for a editor name added with <see cref="AddEditorMapping{TType}(string)"/>
    /// </summary>
    /// <param name="editorName"></param>
    /// <returns></returns>
    string GetEditorMappingKey(string editorName);

    /// <summary>
    /// Gets the property type assembly qualified name stored in the value part of the property map
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <param name="propertyTypeAlias"></param>
    /// <param name="editorAlias"></param>
    /// <returns></returns>
    /// <remarks>This value can be used with <code>Type.GetType</code> to get the type.</remarks>
    string GetPropertyTypeName(string contentTypeAlias, string propertyTypeAlias, string editorAlias);
}

internal class PropertyMap : DictionaryMap, IPropertyMap
{
    /// <summary>
    /// Editor mappings
    /// </summary>
    protected Dictionary<string, string> editorPropertyMap { get; } = [];

    /// <summary>
    /// Alias mappings
    /// </summary>
    protected Dictionary<string, string> aliasPropertyMap { get; } = [];

    /// <summary>
    /// A list of all the types used in the property mapping
    /// </summary>
    protected HashSet<Type> types { get; } = [];

    public void AddEditorMapping<TType>(string editorName) where TType : PropertyValue
    {
        if (AddMapping<TType>(GetEditorMappingKey(editorName), editorPropertyMap))
        {
            AddUsedType<TType>();
        }
    }

    public void AddAliasMapping<TType>(string contentTypeAlias, string propertyTypeAlias) where TType : PropertyValue
    {
        if (AddMapping<TType>(GetAliasMappingKey(contentTypeAlias, propertyTypeAlias), aliasPropertyMap))
        {
            AddUsedType<TType>();
        }
    }

    public void RemoveAliasMapping(string contentTypeAlias, string propertyTypeAlias)
    {
        aliasPropertyMap.Remove(GetAliasMappingKey(contentTypeAlias, propertyTypeAlias));
    }

    public void RemoveEditorMapping(string editorName)
    {
        editorPropertyMap.Remove(GetEditorMappingKey(editorName));
    }

    public bool ContainsEditor(string editorName)
    {
        return editorPropertyMap.ContainsKey(GetEditorMappingKey(editorName));
    }

    public bool ContainsAlias(string contentTypeAlias, string propertyTypeAlias)
    {
        return aliasPropertyMap.ContainsKey(GetAliasMappingKey(contentTypeAlias, propertyTypeAlias));
    }

    public string GetEditorValue(string editorName)
    {
        return editorPropertyMap[GetEditorMappingKey(editorName)];
    }

    public string GetAliasValue(string contentTypeAlias, string propertyTypeAlias)
    {
        return aliasPropertyMap[GetAliasMappingKey(contentTypeAlias, propertyTypeAlias)];
    }

    public HashSet<Type> GetAllTypes()
    {
        return types;
    }

    public string GetEditorMappingKey(string editorName)
    {
        ArgumentNullException.ThrowIfNull(editorName);

        return editorName.ToUpperInvariant();
    }

    public string GetAliasMappingKey(string contentTypeAlias, string propertyTypeAlias)
    {
        ArgumentNullException.ThrowIfNull(contentTypeAlias);
        ArgumentNullException.ThrowIfNull(propertyTypeAlias);

        return $"{contentTypeAlias}&&{propertyTypeAlias}".ToUpperInvariant();
    }

    public string GetPropertyTypeName(string contentTypeAlias, string propertyTypeAlias, string editorAlias)
    {
        string propertyTypeName;
        if (ContainsAlias(contentTypeAlias, propertyTypeAlias))
        {
            propertyTypeName = GetAliasValue(contentTypeAlias, propertyTypeAlias);
        }
        else if (ContainsEditor(editorAlias))
        {
            propertyTypeName = GetEditorValue(editorAlias);
        }
        else
        {
            propertyTypeName = GetEditorValue(PropertyConstants.DefaultKey);
        }
        return propertyTypeName;
    }

    /// <summary>
    /// Adds a type to the types list if it's not already present
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    protected void AddUsedType<TType>() where TType : PropertyValue
    {
        if (!types.Contains(typeof(TType)))
        {
            types.Add(typeof(TType));
        }
    }
}

/// <summary>
/// The base for maps
/// </summary>
internal abstract class DictionaryMap
{
    /// <summary>
    /// Adds a mapping to a dictionary map
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="key"></param>
    /// <param name="map"></param>
    /// <returns>Whether the mapping has been added</returns>
    protected static bool AddMapping<TType>(string key, Dictionary<string, string> map) where TType : class
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(map);

        string? assemblyQualifiedName = typeof(TType).AssemblyQualifiedName;

        if (assemblyQualifiedName == null)
        {
            return false;
        }

        return map.TryAdd(key, assemblyQualifiedName);
    }
}
