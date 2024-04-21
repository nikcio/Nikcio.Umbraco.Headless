using Nikcio.UHeadless.Common.Properties;

namespace Nikcio.UHeadless.UnitTests;

public class PropertyMapTests
{
    public class BasicClass : PropertyValue
    {
        public BasicClass(CreateCommand command) : base(command)
        {
        }
    }

    public class BasicClassAlternate : PropertyValue
    {
        public BasicClassAlternate(CreateCommand command) : base(command)
        {
        }
    }

    [Fact]
    public void AddEditorMapping_BasicClass()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        string? basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        bool containsEditor = propertyMap.ContainsEditor(editorName);
        HashSet<Type> types = propertyMap.GetAllTypes();
        string value = propertyMap.GetEditorValue(editorName);

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
        string? basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        bool containsEditor = propertyMap.ContainsAlias(contentTypeAlias, propertyTypeAlias);
        HashSet<Type> types = propertyMap.GetAllTypes();
        string value = propertyMap.GetAliasValue(contentTypeAlias, propertyTypeAlias);

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

        bool containsEditor = propertyMap.ContainsEditor(editorName);

        Assert.False(containsEditor);
    }

    [Fact]
    public void ContainsAlias_ReturnsFalse_WhenAliasIsNotMapped()
    {
        var propertyMap = new PropertyMap();
        const string contentTypeAlias = "contentType";
        const string propertyTypeAlias = "propertyType";

        bool containsAlias = propertyMap.ContainsAlias(contentTypeAlias, propertyTypeAlias);

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

        HashSet<Type> types = propertyMap.GetAllTypes();

        Assert.Empty(types);
    }

    [Fact]
    public void GetAllTypes_ReturnsCollectionWithOneType_WhenOneMapping()
    {
        var propertyMap = new PropertyMap();
        const string editorName = "Editor";
        string? basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        HashSet<Type> types = propertyMap.GetAllTypes();

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
        string? basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        propertyMap.AddAliasMapping<BasicClassAlternate>(contentTypeAlias, propertyTypeAlias);
        HashSet<Type> types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Equal(2, types.Count);
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
        string? basicClassAlternateAssemblyName = typeof(BasicClassAlternate).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClassAlternate>(editorName);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        HashSet<Type> types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Equal(2, types.Count);
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
        string? basicClassAlternateAssemblyName = typeof(BasicClassAlternate).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClassAlternate>(editorName);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        propertyMap.AddAliasMapping<BasicClassAlternate>(contentTypeAlias, propertyTypeAlias);
        HashSet<Type> types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Equal(2, types.Count);
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
        string? basicClassAssemblyName = typeof(BasicClass).AssemblyQualifiedName;

        propertyMap.AddEditorMapping<BasicClass>(editorName);
        propertyMap.AddAliasMapping<BasicClass>(contentTypeAlias, propertyTypeAlias);
        propertyMap.AddAliasMapping<BasicClassAlternate>(contentTypeAlias, propertyTypeAlias);
        HashSet<Type> types = propertyMap.GetAllTypes();

        Assert.Multiple(() =>
        {
            Assert.Single(types);
            Assert.IsAssignableFrom<IEnumerable<Type>>(types);
            Assert.Equal(basicClassAssemblyName, types.First().AssemblyQualifiedName);
        });
    }
}
