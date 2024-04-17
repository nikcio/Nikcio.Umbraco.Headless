using System.Diagnostics;
using System.Reflection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Manifest;

namespace Nikcio.UHeadless;

/// <summary>
/// Composes the manifest information
/// </summary>
internal class ManifestComposer : IComposer
{
    /// <inheritdoc/>
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ManifestFilters().Append<UHeadlessManifestFilter>();
    }
}

internal sealed class UHeadlessManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        var assembly = Assembly.GetExecutingAssembly();
        manifests.Add(new PackageManifest
        {
            PackageName = "Nikcío.UHeadless",
            Version = assembly != null ? FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion?.ToString() ?? "Unknown" : "Unknown"
        });
    }
}
