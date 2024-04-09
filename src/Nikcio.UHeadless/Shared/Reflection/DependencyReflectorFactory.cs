using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nikcio.UHeadless.Shared.Reflection;

/// <summary>
/// A factory that can create objects with DI
/// </summary>
public interface IDependencyReflectorFactory
{
    /// <summary>
    /// Gets the reflected type with DI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeToReflect">The type to create</param>
    /// <param name="constructorRequiredParamerters">The required parameters on the constructor</param>
    /// <returns></returns>
    T? GetReflectedType<T>(Type typeToReflect, object[] constructorRequiredParamerters) where T : class;
}

/// <inheritdoc/>
internal class DependencyReflectorFactory : IDependencyReflectorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DependencyReflectorFactory> _logger;

    /// <inheritdoc/>
    public DependencyReflectorFactory(IServiceProvider serviceProvider, ILogger<DependencyReflectorFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public T? GetReflectedType<T>(Type typeToReflect, object[] constructorRequiredParamerters)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(typeToReflect);

        ConstructorInfo[] constructors = typeToReflect.GetConstructors();
        if (constructors.Length == 0)
        {
            LogConstructorError(typeToReflect, constructorRequiredParamerters);
            return null;
        }
        ParameterInfo[]? parameters = GetConstructor(constructors, constructorRequiredParamerters)?.GetParameters();
        if (parameters == null)
        {
            LogConstructorError(typeToReflect, constructorRequiredParamerters);
            return null;
        }
        object[]? injectedParamerters = null;
        if (constructorRequiredParamerters == null)
        {
            injectedParamerters = parameters
                .Select(parameter => _serviceProvider.GetService(parameter.ParameterType))
                .OfType<object>()
                .ToArray();
        }
        else
        {
            injectedParamerters = constructorRequiredParamerters
                .Take(parameters.Length)
                .Concat(parameters.Skip(constructorRequiredParamerters.Length).Select(parameter => _serviceProvider.GetService(parameter.ParameterType)))
                .OfType<object>()
                .ToArray();
        }
        return (T?) Activator.CreateInstance(typeToReflect, injectedParamerters);
    }

    /// <summary>
    /// Logs a constructor error
    /// </summary>
    /// <param name="typeToReflect"></param>
    /// <param name="constructorRequiredParamerters"></param>
    private void LogConstructorError(Type typeToReflect, object[] constructorRequiredParamerters)
    {
        string constructorNames = string.Join(", ", constructorRequiredParamerters.Select(item => item.GetType().Name));
        _logger.LogError("Unable to create instance of {TypeToReflectName}. Could not find a constructor with {ConstructorNames} as first argument(s)", typeToReflect.Name, constructorNames);
    }

    /// <summary>
    /// Takes the required paramters from a constructor
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="constructorRequiredParamertersLength"></param>
    /// <returns></returns>
    private static ParameterInfo[] TakeConstructorRequiredParamters(ConstructorInfo constructor, int constructorRequiredParamertersLength)
    {
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length < constructorRequiredParamertersLength)
        {
            return parameters;
        }
        return parameters.Take(constructorRequiredParamertersLength).ToArray();
    }

    /// <summary>
    /// Validates the required parameters from a constructor
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="constructorRequiredParameters"></param>
    /// <returns></returns>
    private static bool ValidateConstructorRequiredParameters(ConstructorInfo constructor, object[] constructorRequiredParameters)
    {
        if (constructorRequiredParameters == null)
        {
            return true;
        }
        ParameterInfo[] parameters = TakeConstructorRequiredParamters(constructor, constructorRequiredParameters.Length);
        for (int i = 0; i < parameters.Length; i++)
        {
            Type requiredParameter = constructorRequiredParameters[i].GetType();
            if (parameters[i].ParameterType != requiredParameter && !parameters[i].ParameterType.IsAssignableFrom(requiredParameter))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Gets a constructor
    /// </summary>
    /// <param name="constructors"></param>
    /// <param name="constructorRequiredParameters"></param>
    /// <returns></returns>
    private static ConstructorInfo? GetConstructor(ConstructorInfo[] constructors, object[] constructorRequiredParameters)
    {
        return Array.Find(constructors, constructor =>
          ValidateConstructorRequiredParameters(constructor, constructorRequiredParameters));
    }
}
