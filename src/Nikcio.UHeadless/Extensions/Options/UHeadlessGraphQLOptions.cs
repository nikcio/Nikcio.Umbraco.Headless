using HotChocolate.Execution.Configuration;

namespace Nikcio.UHeadless.Extensions.Options;

/// <summary>
/// Options for UHeadless GraphQL
/// </summary>
public class UHeadlessGraphQLOptions
{
    /// <summary>
    /// 
    /// </summary>
    public virtual Func<IRequestExecutorBuilder, IRequestExecutorBuilder>? GraphQLExtensions { get; set; }

    /// <summary>
    /// A collection of all the type implementing <see cref="PropertyValue"/>
    /// </summary>
    /// <remarks>
    /// This will be filled automaticly
    /// </remarks>
    public virtual List<Type> PropertyValueTypes { get; set; } = new();
}
