using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Shared.Properties;
using Nikcio.UHeadless.Shared.Reflection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Shared.TypeModules;

/// <summary>
/// Represents the base for creating type modules for the Umbraco types like ContentType and MediaType
/// </summary>
internal abstract class UmbracoTypeModuleBase<TContentType> : ITypeModule
    where TContentType : IContentTypeComposition
{
    /// <summary>
    /// Represents the property map
    /// </summary>
    private readonly IPropertyMap _propertyMap;

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? TypesChanged;

    /// <inheritdoc/>
    protected UmbracoTypeModuleBase(IPropertyMap propertyMap)
    {
        _propertyMap = propertyMap;
    }

    /// <summary>
    /// Gets the content types to register in the GraphQL schema
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<TContentType> GetContentTypes();

    /// <summary>
    /// Call this when the types have changed
    /// </summary>
    /// <param name="eventArgs"></param>
    public void OnTypesChanged(EventArgs eventArgs)
    {
        TypesChanged?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Gets the interface type name for the interface type definition
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <returns></returns>
    protected static string GetInterfaceTypeName(string contentTypeAlias)
    {
        return string.Concat("I", GetObjectTypeName(contentTypeAlias));
    }

    /// <summary>
    /// Gets the object type name for the object type definition
    /// </summary>
    /// <param name="contentTypeAlias"></param>
    /// <returns></returns>
    protected static string GetObjectTypeName(string contentTypeAlias)
    {
        return contentTypeAlias.FirstCharToUpper();
    }

    /// <inheritdoc/>
    public ValueTask<IReadOnlyCollection<ITypeSystemMember>> CreateTypesAsync(IDescriptorContext context, CancellationToken cancellationToken)
    {
        var types = new List<ITypeSystemMember>();

        var objectTypes = new List<ObjectType>();

        AddEmptyPropertyType(objectTypes);

        var contentTypes = GetContentTypes().ToList();

        foreach (TContentType? contentType in contentTypes)
        {
            InterfaceTypeDefinition interfaceTypeDefinition = CreateInterfaceTypeDefinition(contentType);

            if (interfaceTypeDefinition.Fields.Count == 0)
            {
                continue;
            }

            types.Add(InterfaceType.CreateUnsafe(interfaceTypeDefinition));

            ObjectTypeDefinition objectTypeDefinition = CreateObjectTypeDefinition(contentType);

            if (objectTypeDefinition.Fields.Count == 0)
            {
                continue;
            }

            var objectType = ObjectType.CreateUnsafe(objectTypeDefinition);

            objectTypes.Add(objectType);

            types.Add(objectType);
        }

        AddTypedPropertyUnion<TypedProperties>(types, objectTypes, ResolveContentTypeAsObjectType(objectTypes));

        AddTypedPropertyUnion<TypedBlockListContentProperties>(types, objectTypes, ResolveBlockListContentAsObjectType(objectTypes));
        AddTypedPropertyUnion<TypedBlockListSettingsProperties>(types, objectTypes, ResolveBlockListSettingsAsObjectType(objectTypes));

        AddTypedPropertyUnion<TypedBlockGridContentProperties>(types, objectTypes, ResolveBlockGridContentAsObjectType(objectTypes));
        AddTypedPropertyUnion<TypedBlockGridSettingsProperties>(types, objectTypes, ResolveBlockGridSettingsAsObjectType(objectTypes));

        return new ValueTask<IReadOnlyCollection<ITypeSystemMember>>(types);
    }

    private static void AddTypedPropertyUnion<TTypedPropertyUnion>(List<ITypeSystemMember> types, List<ObjectType> objectTypes, ResolveAbstractType resolver)
        where TTypedPropertyUnion : class
    {
        var typedPropertiesUnion = new UnionType<TTypedPropertyUnion>(descriptor =>
        {
            descriptor.ResolveAbstractType(resolver);

            foreach (ObjectType objectType in objectTypes)
            {
                descriptor.Type(objectType);
            }
        });

        types.Add(typedPropertiesUnion);
    }

    /// <summary>
    /// Adds a placeholder empty type that is used when the matching content type doesn't have a type in the schema.
    /// </summary>
    /// <param name="objectTypes"></param>
    private static void AddEmptyPropertyType(List<ObjectType> objectTypes)
    {
        var emptyNamedProperty = new ObjectTypeDefinition($"EmptyPropertyType", "Represents a content type that doesn't have any properties and therefore needs a placeholder");
        emptyNamedProperty.Fields.Add(new ObjectFieldDefinition("Empty_Field", "Placeholder field. Will never hold a value.", type: TypeReference.Parse("String!"), pureResolver: _ => string.Empty));
        objectTypes.Add(ObjectType.CreateUnsafe(emptyNamedProperty));
    }

    private InterfaceTypeDefinition CreateInterfaceTypeDefinition(TContentType contentType)
    {
        var interfaceTypeDefinition = new InterfaceTypeDefinition(GetInterfaceTypeName(contentType.Alias), contentType.Description);

        foreach (IPropertyType property in contentType.CompositionPropertyTypes)
        {
            string propertyTypeName = _propertyMap.GetPropertyTypeName(contentType.Alias, property.Alias, property.PropertyEditorAlias);

            var propertyType = Type.GetType(propertyTypeName);

            if (propertyType == null)
            {
                continue;
            }

            interfaceTypeDefinition.Fields.Add(new InterfaceFieldDefinition(property.Alias, property.Description, TypeReference.Parse(propertyType.Name)));
        }

        return interfaceTypeDefinition;
    }

    private ObjectTypeDefinition CreateObjectTypeDefinition(TContentType contentType)
    {
        var typeDefinition = new ObjectTypeDefinition(GetObjectTypeName(contentType.Alias), contentType.Description);

        foreach (IPropertyType property in contentType.CompositionPropertyTypes)
        {
            string propertyTypeName = _propertyMap.GetPropertyTypeName(contentType.Alias, property.Alias, property.PropertyEditorAlias);

            var propertyType = Type.GetType(propertyTypeName);

            if (propertyType == null)
            {
                continue;
            }

            typeDefinition.Fields.Add(new ObjectFieldDefinition(property.Alias, property.Description, TypeReference.Parse(propertyType.Name), resolver: ResolvePropertyValueAsync));
        }

        foreach (string composite in contentType.CompositionAliases())
        {
            typeDefinition.Interfaces.Add(TypeReference.Parse(GetInterfaceTypeName(composite)));
        }

        typeDefinition.Interfaces.Add(TypeReference.Parse(GetInterfaceTypeName(typeDefinition.Name)));

        return typeDefinition;
    }

    private static ValueTask<object?> ResolvePropertyValueAsync(IResolverContext context)
    {
        BlockGridItem? blockGridItem = context.GetScopedStateOrDefault<BlockGridItem>(ContextDataKeys.BlockGridItem);

        if(blockGridItem != null)
        {
            return ResolveBlockGridPropertyValueAsync(context, blockGridItem);
        }

        BlockListItem? blockListItem = context.GetScopedStateOrDefault<BlockListItem>(ContextDataKeys.BlockListItem);

        if (blockListItem != null)
        {
            return ResolveBlockListPropertyValueAsync(context, blockListItem);
        }

        IPublishedContent? publishedContent = context.GetScopedState<IPublishedContent>(ContextDataKeys.PublishedContent);

        if (publishedContent == null)
        {
            return default;
        }

        IPublishedProperty? publishedProperty = publishedContent.GetProperty(context.Selection.ResponseName);

        if (publishedProperty == null)
        {
            ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
            logger.LogWarning("Property {PropertyName} not found on content type {ContentTypeAlias}.", context.Selection.ResponseName, publishedContent.ContentType.Alias);
            return default;
        }

        var command = new PropertyValue.CreateCommand()
        {
            PublishedProperty = publishedProperty,
            PublishedValueFallback = context.Service<IPublishedValueFallback>(),
            ResolverContext = context
        };

        return ValueTask.FromResult((object?) PropertyValue.CreatePropertyValue(command, context.Service<IPropertyMap>(), context.Service<IDependencyReflectorFactory>()));
    }

    private static ValueTask<object?> ResolveBlockListPropertyValueAsync(IResolverContext context, BlockListItem blockListItem)
    {
        IPublishedProperty? publishedProperty = blockListItem.Content.GetProperty(context.Selection.ResponseName) ?? blockListItem.Settings.GetProperty(context.Selection.ResponseName);

        if (publishedProperty == null)
        {
            ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
            logger.LogWarning("Property {PropertyName} not found on content type {ContentTypeAlias}.", context.Selection.ResponseName, blockListItem.Content.ContentType.Alias);
            return default;
        }

        var command = new PropertyValue.CreateCommand()
        {
            PublishedProperty = publishedProperty,
            PublishedValueFallback = context.Service<IPublishedValueFallback>(),
            ResolverContext = context
        };

        return ValueTask.FromResult((object?) PropertyValue.CreatePropertyValue(command, context.Service<IPropertyMap>(), context.Service<IDependencyReflectorFactory>()));
    }

    private static ValueTask<object?> ResolveBlockGridPropertyValueAsync(IResolverContext context, BlockGridItem blockGridItem)
    {
        IPublishedProperty? publishedProperty = blockGridItem.Content.GetProperty(context.Selection.ResponseName) ?? blockGridItem.Settings.GetProperty(context.Selection.ResponseName);

        if (publishedProperty == null)
        {
            ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
            logger.LogWarning("Property {PropertyName} not found on content type {ContentTypeAlias}.", context.Selection.ResponseName, blockGridItem.Content.ContentType.Alias);
            return default;
        }

        var command = new PropertyValue.CreateCommand()
        {
            PublishedProperty = publishedProperty,
            PublishedValueFallback = context.Service<IPublishedValueFallback>(),
            ResolverContext = context
        };

        return ValueTask.FromResult((object?) PropertyValue.CreatePropertyValue(command, context.Service<IPropertyMap>(), context.Service<IDependencyReflectorFactory>()));
    }

    private static ResolveAbstractType ResolveContentTypeAsObjectType(List<ObjectType> objectTypes)
    {
        return (context, result) =>
        {
            IPublishedContent? publishedContent = context.GetScopedState<IPublishedContent>(ContextDataKeys.PublishedContent);

            if (publishedContent == null)
            {
                ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
                logger.LogWarning("Published content is not available in scoped data.");
                return default;
            }

            if (publishedContent == null)
            {
                return default;
            }

            return objectTypes.Find(type => type.Name == GetObjectTypeName(publishedContent.ContentType.Alias)) ?? objectTypes[0];
        };
    }

    private static ResolveAbstractType ResolveBlockListContentAsObjectType(List<ObjectType> objectTypes)
    {
        return (context, result) =>
        {
            BlockListItem? blockListItem = context.GetScopedState<BlockListItem>(ContextDataKeys.BlockListItem);

            if (blockListItem == null)
            {
                ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
                logger.LogWarning("Block list item is not available in scoped data.");
                return default;
            }

            if (blockListItem == null)
            {
                return default;
            }

            return objectTypes.Find(type => type.Name == GetObjectTypeName(blockListItem.Content.ContentType.Alias)) ?? objectTypes[0];
        };
    }

    private static ResolveAbstractType ResolveBlockListSettingsAsObjectType(List<ObjectType> objectTypes)
    {
        return (context, result) =>
        {
            BlockListItem? blockListItem = context.GetScopedState<BlockListItem>(ContextDataKeys.BlockListItem);

            if (blockListItem == null)
            {
                ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
                logger.LogWarning("Block list item is not available in scoped data.");
                return default;
            }

            if (blockListItem == null)
            {
                return default;
            }

            return objectTypes.Find(type => type.Name == GetObjectTypeName(blockListItem.Settings.ContentType.Alias)) ?? objectTypes[0];
        };
    }

    private static ResolveAbstractType ResolveBlockGridContentAsObjectType(List<ObjectType> objectTypes)
    {
        return (context, result) =>
        {
            BlockGridItem? blockGridItem = context.GetScopedState<BlockGridItem>(ContextDataKeys.BlockGridItem);

            if (blockGridItem == null)
            {
                ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
                logger.LogWarning("Block grid item is not available in scoped data.");
                return default;
            }

            if (blockGridItem == null)
            {
                return default;
            }

            return objectTypes.Find(type => type.Name == GetObjectTypeName(blockGridItem.Content.ContentType.Alias)) ?? objectTypes[0];
        };
    }

    private static ResolveAbstractType ResolveBlockGridSettingsAsObjectType(List<ObjectType> objectTypes)
    {
        return (context, result) =>
        {
            BlockGridItem? blockGridItem = context.GetScopedState<BlockGridItem>(ContextDataKeys.BlockGridItem);

            if (blockGridItem == null)
            {
                ILogger<UmbracoTypeModuleBase<TContentType>> logger = context.Service<ILogger<UmbracoTypeModuleBase<TContentType>>>();
                logger.LogWarning("Block grid item is not available in scoped data.");
                return default;
            }

            if (blockGridItem == null)
            {
                return default;
            }

            return objectTypes.Find(type => type.Name == GetObjectTypeName(blockGridItem.Settings.ContentType.Alias)) ?? objectTypes[0];
        };
    }
}
