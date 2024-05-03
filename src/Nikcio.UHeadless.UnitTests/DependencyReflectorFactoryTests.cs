using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Reflection;
using NSubstitute;

namespace Nikcio.UHeadless.UnitTests;

public class DependencyReflectorFactoryTests
{
    internal sealed class BasicClass
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
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        const string expectedRequiredValue = "Required";
        string[] constructorRequiredParamerters = new[] { expectedRequiredValue };

        BasicClass? reflectedType = reflectorFactory.GetReflectedType<BasicClass>(typeof(BasicClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<BasicClass>(reflectedType);
        Assert.Equal(expectedRequiredValue, reflectedType.Required);
    }

    internal sealed class ServiceClass
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
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(ServiceClass)).Returns(new ServiceClass(expectedRequiredValue, null));
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        string[] constructorRequiredParamerters = new[] { expectedRequiredValue };

        ServiceClass? reflectedType = reflectorFactory.GetReflectedType<ServiceClass>(typeof(ServiceClass), constructorRequiredParamerters);

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

    internal sealed class NoConstructorsClass
    {
#pragma warning disable CS0628 // New protected member declared in sealed type
        protected NoConstructorsClass()
#pragma warning restore CS0628 // New protected member declared in sealed type
        {
        }
    }

    [Fact]
    public void GetReflectedType_NoContructorsClass()
    {
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[] constructorRequiredParamerters = Array.Empty<object>();

        NoConstructorsClass? reflectedType = reflectorFactory.GetReflectedType<NoConstructorsClass>(typeof(NoConstructorsClass), constructorRequiredParamerters);

        Assert.Null(reflectedType);
    }

    internal sealed class NoRequiredParametersClass
    {
        public NoRequiredParametersClass()
        {
        }
    }

    [Fact]
    public void GetReflectedType_NoRequiredParametersClass()
    {
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[] constructorRequiredParamerters = Array.Empty<object>();

        NoRequiredParametersClass? reflectedType = reflectorFactory.GetReflectedType<NoRequiredParametersClass>(typeof(NoRequiredParametersClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<NoRequiredParametersClass>(reflectedType);
    }

    internal sealed class NoRequiredParameters_ServiceClass
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
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(ServiceClass)).Returns(new ServiceClass(expectedRequiredValue, null));
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[] constructorRequiredParamerters = Array.Empty<object>();

        NoRequiredParameters_ServiceClass? reflectedType = reflectorFactory.GetReflectedType<NoRequiredParameters_ServiceClass>(typeof(NoRequiredParameters_ServiceClass), constructorRequiredParamerters);

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
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(ServiceClass)).Returns(new ServiceClass(expectedRequiredValue, null));
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[]? constructorRequiredParamerters = null;

#pragma warning disable CS8604 // Possible null reference argument.
        NoRequiredParameters_ServiceClass? reflectedType = reflectorFactory.GetReflectedType<NoRequiredParameters_ServiceClass>(typeof(NoRequiredParameters_ServiceClass), constructorRequiredParamerters);
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

    internal sealed class IntegerRequiredClass
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
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[] constructorRequiredParamerters = new object[] { new BasicClass("Required") };

        IntegerRequiredClass? reflectedType = reflectorFactory.GetReflectedType<IntegerRequiredClass>(typeof(IntegerRequiredClass), constructorRequiredParamerters);

        Assert.Null(reflectedType);
    }

    [Fact]
    public void GetReflectedType_TooManyRequiredParameters()
    {
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        ILogger<DependencyReflectorFactory> logger = Substitute.For<ILogger<DependencyReflectorFactory>>();
        var reflectorFactory = new DependencyReflectorFactory(serviceProvider, logger);
        object[] constructorRequiredParamerters = new object[] { 1, "TooMuch" };

        IntegerRequiredClass? reflectedType = reflectorFactory.GetReflectedType<IntegerRequiredClass>(typeof(IntegerRequiredClass), constructorRequiredParamerters);

        Assert.NotNull(reflectedType);
        Assert.IsAssignableFrom<IntegerRequiredClass>(reflectedType);
    }
}
