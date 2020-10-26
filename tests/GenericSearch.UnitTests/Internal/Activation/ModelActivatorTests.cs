using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Internal.Activation;
using GenericSearch.Internal.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Activation
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ModelActivatorTests
    {

        [Fact]
        public void Null_Configuration_Throws()
        {
            var activator = new ModelActivator(null);

            activator.Invoking(x => x.CreateInstance(null))
                .Should()
                .ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void FactoryMethod_Succeeds()
        {
            var activator = new ModelActivator(null);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                ModelActivatorConfiguration = new ModelActivatorConfiguration(_ => new Request())
            };

            var result = activator.CreateInstance(source);
            result.Should().BeOfType<Request>();

        }

        [Fact]
        public void FactoryServiceProvider_Succeeds()
        {
            var serviceProvider = new ServiceCollection().AddSingleton<Request>().BuildServiceProvider();
            var httpContext = new Mock<HttpContext>();
            var hca = new Mock<IHttpContextAccessor>();
            hca.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.SetupGet(x => x.RequestServices).Returns(serviceProvider);

            var activator = new ModelActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                ModelActivatorConfiguration = new ModelActivatorConfiguration((sp, x) => sp.GetService(x))
            };

            var result = activator.CreateInstance(source);

            result.Should().BeOfType<Request>();
        }

        [Fact]
        public void FactoryType_Succeeds()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var httpContext = new Mock<HttpContext>();
            var hca = new Mock<IHttpContextAccessor>();
            hca.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.SetupGet(x => x.RequestServices).Returns(serviceProvider);

            var activator = new ModelActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                ModelActivatorConfiguration = new ModelActivatorConfiguration(typeof(Factory))
            };

            var result = activator.CreateInstance(source);

            result.Should().BeOfType<Request>();
        }


        [Fact]
        public void FactoryType_Service_Succeeds()
        {
            var serviceProvider = new ServiceCollection().AddSingleton<Factory>().BuildServiceProvider();
            var httpContext = new Mock<HttpContext>();
            var hca = new Mock<IHttpContextAccessor>();
            hca.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.SetupGet(x => x.RequestServices).Returns(serviceProvider);

            var activator = new ModelActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                ModelActivatorConfiguration = new ModelActivatorConfiguration(typeof(Factory))
            };
            
            var result = activator.CreateInstance(source);

            result.Should().BeOfType<Request>();
        }

        [Fact]
        public void NoFactory_Throws()
        {
            var activator = new ModelActivator(null);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                ModelActivatorConfiguration = new ModelActivatorConfiguration(null, null, null)
            };
            
            activator.Invoking(x => x.CreateInstance(source))
                .Should()
                .ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void UnableToResolveFactory_Throws()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var httpContext = new Mock<HttpContext>();
            var hca = new Mock<IHttpContextAccessor>();
            hca.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.SetupGet(x => x.RequestServices).Returns(serviceProvider);

            var activator = new ModelActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                ModelActivatorConfiguration = new ModelActivatorConfiguration(typeof(Request))
            };
            
            activator.Invoking(x => x.CreateInstance(source))
                .Should()
                .ThrowExactly<NullReferenceException>();
        }


        private class Request
        {
        }

        private class Item
        {
        }

        private class Result
        {
        }

        private class Factory : IModelFactory
        {
            public object Create(Type modelType)
            {
                return Activator.CreateInstance(modelType);
            }
        }
    }
}