using System;
using FluentAssertions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using GenericSearch.Searches.Activation.Factories;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Searches.Activation.Factories
{
    public class SearchActivatorFactoryTests
    {
        [Fact]
        public void Create_RegisteredService_Succeeds()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISearchActivator<TextSearch>, TextSearchActivator>()
                .BuildServiceProvider();
            var factory = new SearchActivatorFactory(serviceProvider);

            var actual = factory.Create(typeof(TextSearch));

            actual.Should().BeOfType<TextSearchActivator>();
        }

        [Fact]
        public void Create_Caching_Succeeds()
        {
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(new TextSearchActivator());

            var factory = new SearchActivatorFactory(mock.Object);

            var actual1 = factory.Create(typeof(TextSearch));
            var actual2 = factory.Create(typeof(TextSearch));

            actual1.Should().BeOfType<TextSearchActivator>();
            actual2.Should().BeOfType<TextSearchActivator>();
            
            mock.Verify(x => x.GetService(It.IsAny<Type>()), Times.Once);
        }

        [Fact]
        public void Create_Fallback_Succeeds()
        {
            var mock = new Mock<IServiceProvider>();
            
            var factory = new SearchActivatorFactory(mock.Object);

            var actual = factory.Create(typeof(TextSearch));

            actual.Should().BeOfType<FallbackSearchActivator>();
            
            mock.Verify(x => x.GetService(It.IsAny<Type>()), Times.Once);
        }
    }
}
