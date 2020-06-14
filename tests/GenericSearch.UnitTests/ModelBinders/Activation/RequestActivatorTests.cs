using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Configuration;
using GenericSearch.ModelBinders.Activation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.ModelBinders.Activation
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class RequestActivatorTests
    {

        [Fact]
        public void Null_Configuration_Throws()
        {
            var activator = new RequestActivator(null);

            activator.Invoking(x => x.Activate(null))
                .Should()
                .ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void FactoryMethod_Succeeds()
        {
            var activator = new RequestActivator(null);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                RequestFactoryConfiguration = new RequestFactoryConfiguration(_ => new Request())
            };

            var result = activator.Activate(source);
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

            var activator = new RequestActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                RequestFactoryConfiguration = new RequestFactoryConfiguration((sp, x) => sp.GetService(x))
            };

            var result = activator.Activate(source);

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

            var activator = new RequestActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                RequestFactoryConfiguration = new RequestFactoryConfiguration(typeof(Factory))
            };

            var result = activator.Activate(source);

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

            var activator = new RequestActivator(hca.Object);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                RequestFactoryConfiguration = new RequestFactoryConfiguration(typeof(Factory))
            };
            
            var result = activator.Activate(source);

            result.Should().BeOfType<Request>();
        }

        [Fact]
        public void NoFactory_Throws()
        {
            var activator = new RequestActivator(null);
            var source = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result))
            {
                RequestFactoryConfiguration = new RequestFactoryConfiguration()
            };
            
            activator.Invoking(x => x.Activate(source))
                .Should()
                .ThrowExactly<ArgumentException>();
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

        private class Factory : IRequestFactory
        {
            public object Create(Type requestType)
            {
                return Activator.CreateInstance(requestType);
            }
        }
    }
}