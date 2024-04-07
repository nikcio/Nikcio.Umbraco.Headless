using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Nikcio.UHeadless;

public static class PureResolverExtensions
{
    /// <summary>
    /// Gets the scoped state for the specified name, or throws if the state does not exist.
    /// </summary>
    /// <typeparam name="T">The type of the state.</typeparam>
    /// <param name="context">The resolver context.</param>
    /// <param name="name">The name of the state.</param>
    /// <returns>Returns the scoped state for the specified name.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static T? GetScopedState<T>(this IPureResolverContext context, string name)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrEmpty(name);

        if (context.ScopedContextData.TryGetValue(name, out object? value))
        {
            if (value == null)
            {
                return default;
            }

            if (value is T typedValue)
            {
                return typedValue;
            }
        }

        throw new ArgumentException($"State with the provided name: '{name}' does not exist.");
    }

    /// <summary>
    /// Sets the scoped state for name to the specified value. State set previously using
    /// the same name will be overwritten.
    /// </summary>
    /// <typeparam name="T">The type of the state.</typeparam>
    /// <param name="context">The resolver context.</param>
    /// <param name="name">The name of the state.</param>
    /// <param name="value">The new state value.</param>
    public static void SetScopedState<T>(this IResolverContext context, string name, T value)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrEmpty(name);

        context.ScopedContextData = context.ScopedContextData.SetItem(name, value);
    }
}
