using Nikcio.UHeadless.Base.Properties.Models;
using Nikcio.UHeadless.Core.Constants;
using Nikcio.UHeadless.Core.Maps;

namespace Nikcio.UHeadless.Base.Properties.Maps;

/// <inheritdoc/>
public class PropertyMap : DictionaryMap, IPropertyMap
{
    /// <summary>
    /// Editor mappings
    /// </summary>
    protected Dictionary<string, string> editorPropertyMap { get; } = new();

    /// <summary>
    /// Alias mappings
    /// </summary>
    protected Dictionary<string, string> aliasPropertyMap { get; } = new();

    /// <summary>
    /// A list of all the types used in the property mapping
    /// </summary>
    protected HashSet<Type> types { get; } = new();

    /// <inheritdoc/>
    public virtual void AddEditorMapping<TType>(string editorName) where TType : PropertyValue
    {
        if (AddMapping<TType>(GetEditorMappingKey(editorName), editorPropertyMap))
        {
            AddUsedType<TType>();
        }
    }

    /// <inheritdoc/>
    public virtual void AddAliasMapping<TType>(string contentTypeAlias, string propertyTypeAlias) where TType : PropertyValue
    {
        if (AddMapping<TType>(GetAliasMappingKey(contentTypeAlias, propertyTypeAlias), aliasPropertyMap))
        {
            AddUsedType<TType>();
        }
    }

    /// <inheritdoc/>
    public virtual bool ContainsEditor(string editorName)
    {
        return editorPropertyMap.ContainsKey(GetEditorMappingKey(editorName));
    }

    /// <inheritdoc/>
    public virtual bool ContainsAlias(string contentTypeAlias, string propertyTypeAlias)
    {
        return aliasPropertyMap.ContainsKey(GetAliasMappingKey(contentTypeAlias, propertyTypeAlias));
    }

    /// <inheritdoc/>
    public virtual string GetEditorValue(string editorName)
    {
        return editorPropertyMap[GetEditorMappingKey(editorName)];
    }

    /// <inheritdoc/>
    public virtual string GetAliasValue(string contentTypeAlias, string propertyTypeAlias)
    {
        return aliasPropertyMap[GetAliasMappingKey(contentTypeAlias, propertyTypeAlias)];
    }

    /// <inheritdoc/>
    public virtual IEnumerable<Type> GetAllTypes()
    {
        return types;
    }

    /// <inheritdoc/>
    public virtual string GetEditorMappingKey(string editorName)
    {
        ArgumentNullException.ThrowIfNull(editorName, nameof(editorName));

        return editorName.ToUpperInvariant();
    }

    /// <inheritdoc/>
    public virtual string GetAliasMappingKey(string contentTypeAlias, string propertyTypeAlias)
    {
        ArgumentNullException.ThrowIfNull(contentTypeAlias, nameof(contentTypeAlias));
        ArgumentNullException.ThrowIfNull(propertyTypeAlias, nameof(propertyTypeAlias));

        return $"{contentTypeAlias}&&{propertyTypeAlias}".ToUpperInvariant();
    }

    /// <inheritdoc/>
    public virtual string GetPropertyTypeName(string contentTypeAlias, string propertyTypeAlias, string editorAlias)
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
    protected virtual void AddUsedType<TType>() where TType : PropertyValue
    {
        if (!types.Contains(typeof(TType)))
        {
            types.Add(typeof(TType));
        }
    }
}
