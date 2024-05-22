using Umbraco.Cms.Core.Composing;

namespace Nikcio.UHeadless.IntegrationTests;

/// <summary>
/// Sets up UHeadless based on the active <see cref="ApplicationFactoryBase"/>
/// </summary>
public class UHeadlessSetupComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        string? headlessSetupTypeName = builder.Config[nameof(ApplicationFactoryBase.UHeadlessSetup)];

        ArgumentException.ThrowIfNullOrWhiteSpace(headlessSetupTypeName);

        var headlessSetup = (UHeadlessSetup?) Activator.CreateInstance(Type.GetType(headlessSetupTypeName) ?? throw new InvalidOperationException("Couldn't create the UHeadless test setup"));

        ArgumentNullException.ThrowIfNull(headlessSetup);

        builder.AddUHeadless(headlessSetup.GetSetup());
    }
}
