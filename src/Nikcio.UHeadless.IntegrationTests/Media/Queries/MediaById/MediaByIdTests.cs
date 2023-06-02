using Nikcio.UHeadless.IntegrationTests.Extensions;
using StrawberryShake;

namespace Nikcio.UHeadless.IntegrationTests.Media.Queries;

public class MediaByIdTests : IntegrationTestBase
{
    private readonly Setup _setup = new();

    [TearDown]
    public void TearDown()
    {
        _setup.Dispose();
    }

    [Test]
    public async Task GetGeneralMediaById_Test()
    {
        var rootResult = await _setup.UHeadlessClient.GetGeneralMediaAtRoot.ExecuteAsync();

        rootResult.Errors.EnsureNoErrors();
        Assert.That(rootResult, Is.Not.Null);
        Assert.That(rootResult.Data, Is.Not.Null);
        Assert.That(rootResult.Data!.MediaAtRoot, Is.Not.Null);
        Assert.That(rootResult.Data!.MediaAtRoot!.Nodes, Is.Not.Empty);
        Assert.That(rootResult.Data!.MediaAtRoot!.Nodes!.All(node => node!.Id != null), Is.True);

        for (int i = 0; i < rootResult.Data!.MediaAtRoot!.Nodes!.Count; i++)
        {
            var result = await _setup.UHeadlessClient.GetGeneralMediaById.ExecuteAsync(rootResult.Data!.MediaAtRoot!.Nodes[i]!.Id!.Value);

            result.Errors.EnsureNoErrors();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.MediaById, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data!.MediaById!.Id ?? 0, Is.GreaterThan(0));
                Assert.That(result.Data!.MediaById!.Key, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(result.Data!.MediaById!.Key, Is.Not.Empty);
                Assert.That(result.Data!.MediaById!.Name, Is.Not.Empty);
                Assert.That(result.Data!.MediaById!.CreatorId, Is.EqualTo(-1));
                Assert.That(result.Data!.MediaById!.WriterId, Is.EqualTo(-1));
                Assert.That(result.Data!.MediaById!.Properties?.All(property => property != null && !string.IsNullOrEmpty(property.Alias)), Is.True);
                Assert.That(result.Data!.MediaById!.ItemType.ToString(), Is.Not.Empty);
                Assert.That(result.Data!.MediaById!.Level, Is.GreaterThan(0));
                Assert.That(result.Data!.MediaById!.SortOrder, Is.GreaterThan(-1));
                Assert.That(result.Data!.MediaById!.Url, Is.Not.Null);
                Assert.That(result.Data!.MediaById!.AbsoluteUrl, Is.Not.Null);
                Assert.That(result.Data!.MediaById!.Children?.All(child => !string.IsNullOrEmpty(child!.Name)), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.CreatorId == -1), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.WriterId == -1), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.Properties != null && child.Properties.All(property => !string.IsNullOrEmpty(property!.Alias))), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => !string.IsNullOrEmpty(child!.ItemType.ToString())), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.Level > result.Data!.MediaById!.Level), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.Parent != null), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => !string.IsNullOrEmpty(child!.Parent!.Name)), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.SortOrder > -1), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.Url != null), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.AbsoluteUrl != null), Is.True);
            });
        }
    }

    [Test]
    public async Task GetNodeIdMediaById_Test()
    {
        var rootResult = await _setup.UHeadlessClient.GetGeneralMediaAtRoot.ExecuteAsync();

        rootResult.Errors.EnsureNoErrors();
        Assert.That(rootResult, Is.Not.Null);
        Assert.That(rootResult.Data, Is.Not.Null);
        Assert.That(rootResult.Data!.MediaAtRoot, Is.Not.Null);
        Assert.That(rootResult.Data!.MediaAtRoot!.Nodes, Is.Not.Empty);
        Assert.That(rootResult.Data!.MediaAtRoot!.Nodes!.All(node => node!.Id != null), Is.True);

        for (int i = 0; i < rootResult.Data!.MediaAtRoot!.Nodes!.Count; i++)
        {
            var result = await _setup.UHeadlessClient.GetNodeIdMediaById.ExecuteAsync(rootResult.Data!.MediaAtRoot!.Nodes[i]!.Id!.Value);

            result.Errors.EnsureNoErrors();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.MediaById, Is.Not.Null);
            Assert.That(result.Data!.MediaById!.Id, Is.GreaterThan(0));
        }
    }

    [Test]
    public async Task GetPropertiesMediaById_Test()
    {
        var rootResult = await _setup.UHeadlessClient.GetGeneralMediaAtRoot.ExecuteAsync();

        rootResult.Errors.EnsureNoErrors();
        Assert.That(rootResult, Is.Not.Null);
        Assert.That(rootResult.Data, Is.Not.Null);
        Assert.That(rootResult.Data!.MediaAtRoot, Is.Not.Null);
        Assert.That(rootResult.Data!.MediaAtRoot!.Nodes, Is.Not.Empty);
        Assert.That(rootResult.Data!.MediaAtRoot!.Nodes!.All(node => node!.Id != null), Is.True);

        for (int i = 0; i < rootResult.Data!.MediaAtRoot!.Nodes!.Count; i++)
        {
            var result = await _setup.UHeadlessClient.GetPropertiesMediaById.ExecuteAsync(rootResult.Data!.MediaAtRoot!.Nodes[i]!.Id!.Value);

            result.Errors.EnsureNoErrors();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.MediaById, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data!.MediaById!.Properties, Is.Not.Null);
                Assert.That(result.Data!.MediaById!.Properties!.All(property => !string.IsNullOrEmpty(property!.Alias)), Is.True);
                Assert.That(result.Data!.MediaById!.Properties!.All(property => !string.IsNullOrEmpty(property!.EditorAlias)), Is.True);
                Assert.That(result.Data!.MediaById!.Properties!.All(property => property!.Value != null), Is.True);
                Assert.That(result.Data!.MediaById!.Properties!.All(property => IsPropertyValueValid(property!.Value!)), Is.True);
                Assert.That(result.Data!.MediaById!.Children?.All(child => child!.Properties != null && child.Properties.All(property => IsPropertyValueValid(property!.Value!))), Is.True);
            });
        }
    }

    private static bool IsPropertyValueValid(IGetPropertiesMediaById_MediaById_Properties_Value value)
    {
        if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicBlockListModel blockListModel)
        {
            Assert.That(blockListModel.Blocks, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(blockListModel.Blocks!.All(block => block.ContentProperties != null));
                Assert.That(blockListModel.Blocks!.All(block => block.ContentProperties.All(property => property!.Value != null)));
            });
            // TODO: MediaProperty validation
            // TODO: SettingProperty validation
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicContentPicker contentPicker)
        {
            Assert.That(contentPicker.ContentList, Is.Not.Null);
            Assert.That(contentPicker.ContentList!.All(item => item.Id > 0), Is.True);
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicDateTimePicker dateTimePicker)
        {
            Assert.That(dateTimePicker.DateTime == null || dateTimePicker.DateTime != default, Is.True);
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicLabel label)
        {
            Assert.That(label.Label, Is.Not.Null);
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicMediaPicker mediaPicker)
        {
            Assert.That(mediaPicker.MediaItems, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(mediaPicker.MediaItems.All(item => item.Id > 0));
                Assert.That(mediaPicker.MediaItems.All(item => !string.IsNullOrEmpty(item.Url)));
            });
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicMemberPicker memberPicker)
        {
            Assert.That(memberPicker.Members, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(memberPicker.Members.All(member => member.Id > 0));
                Assert.That(memberPicker.Members.All(member => !string.IsNullOrEmpty(member.Name)));
                Assert.That(memberPicker.Members.All(member => member.Properties != null));
            });
            // TODO: Member property validation
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicMultiUrlPicker multiUrlPicker)
        {
            Assert.That(multiUrlPicker.Links, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(multiUrlPicker.Links.All(link => !string.IsNullOrEmpty(link.Name)));
                Assert.That(multiUrlPicker.Links.All(link => !string.IsNullOrEmpty(link.Url)));
            });
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicPropertyValue basicValue)
        {
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicRichText richText)
        {
            Assert.Multiple(() =>
            {
                Assert.That(richText.RichText, Is.Not.Null);
                Assert.That(richText.SourceValue == null || !string.IsNullOrEmpty(richText.RichText), Is.True);
            });
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Properties_Value_BasicUnsupportedPropertyValue unsupportedValue)
        {
            Assert.That(unsupportedValue.Message, Is.Not.Null);
            Assert.That(unsupportedValue.Message, Is.Not.Empty);
            return true;
        }

        return false;
    }

    private static bool IsPropertyValueValid(IGetPropertiesMediaById_MediaById_Children_Properties_Value value)
    {
        if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicBlockListModel blockListModel)
        {
            Assert.That(blockListModel.Blocks, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(blockListModel.Blocks!.All(block => block.ContentProperties != null));
                Assert.That(blockListModel.Blocks!.All(block => block.ContentProperties.All(property => property!.Value != null)));
            });
            // TODO: MediaProperty validation
            // TODO: SettingProperty validation
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicContentPicker contentPicker)
        {
            Assert.That(contentPicker.ContentList, Is.Not.Null);
            Assert.That(contentPicker.ContentList!.All(item => item.Id > 0), Is.True);
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicDateTimePicker dateTimePicker)
        {
            Assert.That(dateTimePicker.DateTime == null || dateTimePicker.DateTime != default, Is.True);
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicLabel label)
        {
            Assert.That(label.Label, Is.Not.Null);
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicMediaPicker mediaPicker)
        {
            Assert.That(mediaPicker.MediaItems, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(mediaPicker.MediaItems.All(item => item.Id > 0));
                Assert.That(mediaPicker.MediaItems.All(item => !string.IsNullOrEmpty(item.Url)));
            });
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicMemberPicker memberPicker)
        {
            Assert.That(memberPicker.Members, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(memberPicker.Members.All(member => member.Id > 0));
                Assert.That(memberPicker.Members.All(member => !string.IsNullOrEmpty(member.Name)));
                Assert.That(memberPicker.Members.All(member => member.Properties != null));
            });
            // TODO: Member property validation
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicMultiUrlPicker multiUrlPicker)
        {
            Assert.That(multiUrlPicker.Links, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(multiUrlPicker.Links.All(link => !string.IsNullOrEmpty(link.Name)));
                Assert.That(multiUrlPicker.Links.All(link => !string.IsNullOrEmpty(link.Url)));
            });
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicPropertyValue basicValue)
        {
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicRichText richText)
        {
            Assert.Multiple(() =>
            {
                Assert.That(richText.RichText, Is.Not.Null);
                Assert.That(richText.SourceValue == null || !string.IsNullOrEmpty(richText.RichText), Is.True);
            });
            return true;
        } else if (value is IGetPropertiesMediaById_MediaById_Children_Properties_Value_BasicUnsupportedPropertyValue unsupportedValue)
        {
            Assert.That(unsupportedValue.Message, Is.Not.Null);
            Assert.That(unsupportedValue.Message, Is.Not.Empty);
            return true;
        }

        return false;
    }
}