using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Core.Reflection.Factories;
using NSubstitute;

namespace Nikcio.UHeadless.Base.Tests.Reflection;

public class DependencyReflectorFactoryTests
{
    internal class BasicClass
    {
        public string Required { get; }
        public BasicClass(string required)
        {
            Required = required;
        }
    }

    [Fact]
    public void GetReflectedType_BasicClass()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        const string expectedRequiredValue = "Required";
        var constructorRequiredParamerters = new[] { expectedRequiredValue };

        var reflectedType = reflectorFactory.GetReflectedType<BasicClass>(typeof(BasicClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<BasicClass>(reflectedType);
        Assert.Equal(expectedRequiredValue, reflectedType.Required);
    }

    internal class ServiceClass
    {
        public string Required { get; }
        public ServiceClass? Service { get; }

        public ServiceClass(string required, ServiceClass? serviceClass)
        {
            Required = required;
            Service = serviceClass;
        }
    }

    [Fact]
    public void GetReflectedType_ServiceClass()
    {
        const string expectedRequiredValue = "Required";
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(ServiceClass)).Returns(new ServiceClass(expectedRequiredValue, null));
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        var constructorRequiredParamerters = new[] { expectedRequiredValue };

        var reflectedType = reflectorFactory.GetReflectedType<ServiceClass>(typeof(ServiceClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<ServiceClass>(reflectedType);
        Assert.Multiple(() =>
        {
            Assert.Equal(expectedRequiredValue, reflectedType.Required);
            Assert.NotNull(reflectedType.Service);
            Assert.IsAssignableFrom<ServiceClass>(reflectedType.Service);
        });
        Assert.Multiple(() =>
        {
            Assert.NotNull(reflectedType.Service);
            Assert.Equal(expectedRequiredValue, reflectedType.Service.Required);
            Assert.Null(reflectedType.Service.Service);
        });
    }

    internal class NoConstructorsClass
    {
        protected NoConstructorsClass()
        {
        }
    }

    [Fact]
    public void GetReflectedType_NoContructorsClass()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        var constructorRequiredParamerters = Array.Empty<object>();

        var reflectedType = reflectorFactory.GetReflectedType<NoConstructorsClass>(typeof(NoConstructorsClass), constructorRequiredParamerters);

        Assert.Null(reflectedType);
    }

    internal class NoRequiredParametersClass
    {
        public NoRequiredParametersClass()
        {
        }
    }

    [Fact]
    public void GetReflectedType_NoRequiredParametersClass()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        var constructorRequiredParamerters = Array.Empty<object>();

        var reflectedType = reflectorFactory.GetReflectedType<NoRequiredParametersClass>(typeof(NoRequiredParametersClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<NoRequiredParametersClass>(reflectedType);
    }

    internal class NoRequiredParameters_ServiceClass
    {
        public ServiceClass Service { get; }

        public NoRequiredParameters_ServiceClass(ServiceClass service)
        {
            Service = service;
        }
    }

    [Fact]
    public void GetReflectedType_NoRequiredParameters_ServiceClass()
    {
        const string expectedRequiredValue = "Required";
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(ServiceClass)).Returns(new ServiceClass(expectedRequiredValue, null));
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        var constructorRequiredParamerters = Array.Empty<object>();

        var reflectedType = reflectorFactory.GetReflectedType<NoRequiredParameters_ServiceClass>(typeof(NoRequiredParameters_ServiceClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<NoRequiredParameters_ServiceClass>(reflectedType);
        Assert.IsAssignableFrom<ServiceClass>(reflectedType.Service);
        Assert.Multiple(() =>
        {
            Assert.NotNull(reflectedType.Service);
            Assert.Equal(expectedRequiredValue, reflectedType.Service.Required);
            Assert.Null(reflectedType.Service.Service);
        });
    }

    [Fact]
    public void GetReflectedType_RequiredParametersIsNull_ServiceClass()
    {
        const string expectedRequiredValue = "Required";
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(ServiceClass)).Returns(new ServiceClass(expectedRequiredValue, null));
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[]? constructorRequiredParamerters = null;

#pragma warning disable CS8604 // Possible null reference argument.
        var reflectedType = reflectorFactory.GetReflectedType<NoRequiredParameters_ServiceClass>(typeof(NoRequiredParameters_ServiceClass), constructorRequiredParamerters);
#pragma warning restore CS8604 // Possible null reference argument.

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<NoRequiredParameters_ServiceClass>(reflectedType);
        Assert.IsAssignableFrom<ServiceClass>(reflectedType.Service);
        Assert.Multiple(() =>
        {
            Assert.NotNull(reflectedType.Service);
            Assert.Equal(expectedRequiredValue, reflectedType.Service.Required);
            Assert.Null(reflectedType.Service.Service);
        });
    }

    internal class IntegerRequiredClass
    {
        public int Required { get; set; }
        public IntegerRequiredClass(int required)
        {
            Required = required;
        }
    }

    [Fact]
    public void GetReflectedType_WrongRequiredParameters()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        var constructorRequiredParamerters = new object[] { new BasicClass("Required") };

        var reflectedType = reflectorFactory.GetReflectedType<IntegerRequiredClass>(typeof(IntegerRequiredClass), constructorRequiredParamerters);

        Assert.Null(reflectedType);
    }

    [Fact]
    public void GetReflectedType_TooManyRequiredParameters()
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        var constructorRequiredParamerters = new object[] { 1, "TooMuch" };

        var reflectedType = reflectorFactory.GetReflectedType<IntegerRequiredClass>(typeof(IntegerRequiredClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<IntegerRequiredClass>(reflectedType);
    }
}
