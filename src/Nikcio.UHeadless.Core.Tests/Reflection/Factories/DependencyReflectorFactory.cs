﻿using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Core.Reflection.Factories;

namespace Nikcio.UHeadless.Core.Tests.Reflection.Factories {
    public class DependencyReflectorFactoryTests {
        internal class BasicClass {
            public string Required { get; }
            public BasicClass(string required) {
                Required = required;
            }
        }

        internal class ServiceClass {
            public string Required { get; }
            public ServiceClass? Service { get; }

            public ServiceClass(string required, ServiceClass? serviceClass) {
                Required = required;
                Service = serviceClass;
            }
        }

        [Test]
        public void GetReflectedType_BasicClass() {
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<DependencyReflectorFactory>>();
            var reflectorFactory = new DependencyReflectorFactory(serviceProvider.Object, logger.Object);
            var expectedRequiredValue = "Required";
            var constructorRequiredParamerters = new[] { expectedRequiredValue };

            var reflectedType = reflectorFactory.GetReflectedType<BasicClass>(typeof(BasicClass), constructorRequiredParamerters);

            Assert.That(reflectedType, Is.InstanceOf(typeof(BasicClass)));
            Assert.That(reflectedType.Required, Is.EqualTo(expectedRequiredValue));
        }

        [Test]
        public void GetReflectedType_ServiceClass() {
            var expectedRequiredValue = "Required";
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(ServiceClass)))
                .Returns(new ServiceClass(expectedRequiredValue, null));
            var logger = new Mock<ILogger<DependencyReflectorFactory>>();
            var reflectorFactory = new DependencyReflectorFactory(serviceProvider.Object, logger.Object);
            var constructorRequiredParamerters = new[] { expectedRequiredValue };

            var reflectedType = reflectorFactory.GetReflectedType<ServiceClass>(typeof(ServiceClass), constructorRequiredParamerters);

            Assert.That(reflectedType, Is.InstanceOf(typeof(ServiceClass)));
            Assert.Multiple(() => {
                Assert.That(reflectedType.Required, Is.EqualTo(expectedRequiredValue));
                Assert.That(reflectedType.Service, Is.InstanceOf(typeof(ServiceClass)));
            });
            Assert.Multiple(() => {
                Assert.That(reflectedType.Service, Is.Not.Null);
                Assert.That(reflectedType.Service?.Required, Is.EqualTo(expectedRequiredValue));
                Assert.That(reflectedType.Service?.Service, Is.EqualTo(null));
            });
        }
    }
}
