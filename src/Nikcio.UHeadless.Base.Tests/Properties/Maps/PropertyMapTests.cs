using Nikcio.UHeadless.Base.Properties.Commands;
using Nikcio.UHeadless.Base.Properties.Maps;
using Nikcio.UHeadless.Base.Properties.Models;

namespace Nikcio.UHeadless.Base.Tests.Properties.Maps;

public class PropertyMapTests
{
    public class BasicClass : PropertyValue
    {
        public BasicClass(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
        {
        }
    }

    public class BasicClassAlternate : PropertyValue
    {
        public BasicClassAlternate(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
        {
        }
    }

    [Fact]
    public void AddEditorMapping_BasicClass()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        var basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        var containsEditor = propertyMap.ContainsEditor(editorName);
        var types = propertyMap.GetAllTypes();
        var value = propertyMap.GetEditorValue(editorName);

        Assert.True(containsEditor);
        Assert.Multiple(() =>
        {
            Assert.Single(types);
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
        });
        Assert.Equal(basicClassAssemblyName, value);
    }

    [Fact]
    public void AddAliasMapping_BasicClass()
    {
        var propertyMap = new PropertyMap();
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";
        var basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        var containsEditor = propertyMap.ContainsAlias(contentTypeAlias, propertyTypeAlias);
        var types = propertyMap.GetAllTypes();
        var value = propertyMap.GetAliasValue(contentTypeAlias, propertyTypeAlias);

        Assert.True(containsEditor);
        Assert.Multiple(() =>
        {
            Assert.Single(types);
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
        });
        Assert.Equal(basicClassAssemblyName, value);
    }

    [Fact]
    public void ContainsEditor_ReturnsFalse_WhenEditorIsNotMapped()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";

        var containsEditor = propertyMap.ContainsEditor(editorName);

        Assert.False(containsEditor);
    }

    [Fact]
    public void ContainsAlias_ReturnsFalse_WhenAliasIsNotMapped()
    {
        var propertyMap = new PropertyMap();
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";

        var containsAlias = propertyMap.ContainsAlias(contentTypeAlias, propertyTypeAlias);

        Assert.False(containsAlias);
    }

    [Fact]
    public void GetEditorValue_ThrowsException_WhenEditorIsNotMapped()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";

        Assert.Throws<KeyNotFoundException>(() => propertyMap.GetEditorValue(editorName));
    }

    [Fact]
    public void GetAliasValue_ThrowsException_WhenAliasIsNotMapped()
    {
        var propertyMap = new PropertyMap();
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";

        Assert.Throws<KeyNotFoundException>(() => propertyMap.GetAliasValue(contentTypeAlias, propertyTypeAlias));
    }

    [Fact]
    public void GetAllTypes_ReturnsEmptyCollection_WhenNoMappings()
    {
        var propertyMap = new PropertyMap();

        var types = propertyMap.GetAllTypes();

        Assert.Empty(types);
    }

    [Fact]
    public void GetAllTypes_ReturnsCollectionWithOneType_WhenOneMapping()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        var basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        var types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Single(types);
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
            Assert.Equal(basicClassAssemblyName, types.First().AssemblyQualifiedName);
        });
    }

    [Fact]
    public void GetAllTypes_ReturnsCollectionWithTwoTypes_WhenTwoMappings()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";
        var basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        propertyMap.AddAliasMapping<BasicClassAlternate>(contentTypeAlias, propertyTypeAlias);
        var types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Equal(2, types.Count());
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
            Assert.Equal(basicClassAssemblyName, types.First().AssemblyQualifiedName);
        });
    }

    [Fact]
    public void GetAllTypes_ReturnsCollectionWithTwoTypes_WhenTwoMappingsWithSameType()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";
        var basicClassAlternateAssemblyName = typeof(BasicClassAlternate).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClassAlternate>(editorName);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        var types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Equal(2, types.Count());
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
            Assert.Equal(basicClassAlternateAssemblyName, types.First().AssemblyQualifiedName);
        });
    }

    [Fact]
    public void GetAllTypes_ReturnsCollectionWithTwoTypes_WhenTwoMappingsWithSameTypeAndOneDifferent()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";
        var basicClassAlternateAssemblyName = typeof(BasicClassAlternate).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClassAlternate>(editorName);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        propertyMap.AddAliasMapping<BasicClassAlternate>(contentTypeAlias, propertyTypeAlias);
        var types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Equal(2, types.Count());
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
            Assert.Equal(basicClassAlternateAssemblyName, types.First().AssemblyQualifiedName);
        });
    }

    [Fact]
    public void GetAllTypes_ReturnsCollectionWithOneType_WhenTwoMappingsWithSameTypeAndOneDifferentAndOneEditor()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";
        var basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        propertyMap.AddAliasMapping<BasicClassAlternate>(contentTypeAlias, propertyTypeAlias);
        var types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Single(types);
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
            Assert.Equal(basicClassAssemblyName, types.First().AssemblyQualifiedName);
        });
    }
}
