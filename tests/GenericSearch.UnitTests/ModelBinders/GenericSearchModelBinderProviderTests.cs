using System;
using System.Collections.Generic;
using FluentAssertions;
using GenericSearch.Configuration;
using GenericSearch.Definition;
using GenericSearch.Internal;
using GenericSearch.ModelBinders;
using GenericSearch.ModelBinders.Activation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.ModelBinders
{
    public class GenericSearchModelBinderProviderTests
    {
        private readonly Mock<GenericSearchOptions> options = new Mock<GenericSearchOptions>();
        private readonly Mock<IOptions<GenericSearchOptions>> optionsMock = new Mock<IOptions<GenericSearchOptions>>();

        public GenericSearchModelBinderProviderTests()
        {
            optionsMock.SetupGet(x => x.Value).Returns(options.Object);
        }

        [Fact]
        public void Null_Configuration_Succeeds()
        {
            var configurationProvider = new ListConfigurationProvider(new List<IListDefinitionSource>(), null, optionsMock.Object);
            var requestActivator = new Mock<IModelActivator>();
            var requestPropertyActivator = new Mock<ISearchPropertyActivator>();

            var services = new ServiceCollection()
                .AddSingleton<IListConfigurationProvider>(configurationProvider)
                .AddSingleton(requestActivator.Object)
                .AddSingleton(requestPropertyActivator.Object)
                .BuildServiceProvider();

            var context = new TestModelBinderContext(typeof(Request), services);
            var fallbackBinder = new Mock<IModelBinderProvider>();
            
            var provider = new GenericSearchModelBinderProvider(fallbackBinder.Object);

            var modelBinder = provider.GetBinder(context);
            modelBinder.Should().BeNull();
        }

        [Fact]
        public void Valid_Configuration_Succeeds()
        {
            var configuration = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result));

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(configuration);
            var requestActivator = new Mock<IModelActivator>();
            var requestPropertyActivator = new Mock<ISearchPropertyActivator>();
            var modelCache = new Mock<IModelCache>();

            var httpContext = new Mock<HttpContext>();
            var accessor = new TestHttpContextAccessor(httpContext.Object);

            var services = new ServiceCollection()
                .AddSingleton(configurationProvider.Object)
                .AddSingleton(requestActivator.Object)
                .AddSingleton(requestPropertyActivator.Object)
                .AddSingleton(modelCache.Object)
                .AddSingleton<IHttpContextAccessor>(accessor)
                .BuildServiceProvider();

            httpContext.SetupGet(x => x.RequestServices).Returns(services);

            var context = new TestModelBinderContext(typeof(Request), services);

            var fallbackModelBinder = new Mock<IModelBinder>();
            var fallbackModelBinderProvider = new Mock<IModelBinderProvider>();
            fallbackModelBinderProvider.Setup(x => x.GetBinder(It.IsAny<ModelBinderProviderContext>())).Returns(fallbackModelBinder.Object);

            var provider = new GenericSearchModelBinderProvider(fallbackModelBinderProvider.Object);

            var modelBinder = provider.GetBinder(context);
            modelBinder.Should().BeOfType<GenericSearchModelBinder>();
        }

        private class TestHttpContextAccessor : IHttpContextAccessor
        {
            public TestHttpContextAccessor(HttpContext httpContext) => HttpContext = httpContext;
            public HttpContext HttpContext { get; set; }
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
    }
}