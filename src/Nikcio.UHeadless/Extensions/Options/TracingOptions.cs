using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Options;

namespace Nikcio.UHeadless.Extensions.Options;

/// <summary>
/// Apollo tracing options
/// </summary>
public class TracingOptions
{
    /// <summary>
    /// TracingPreference
    /// </summary>
    public virtual TracingPreference? TracingPreference { get; set; }

    /// <summary>
    /// ITimestampProvider
    /// </summary>
    public virtual ITimestampProvider? TimestampProvider { get; set; }
}
