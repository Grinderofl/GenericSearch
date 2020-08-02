using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Configuration.Factories;
using GenericSearch.Definition;
using GenericSearch.Definition.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class RequestFactoryConfigurationFactoryTests : ConfigurationFactoryTestBase
    {
        private ModelActivatorConfigurationFactory Factory => new ModelActivatorConfigurationFactory(Options);

        [Fact]
        public void NoDefinition_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            
            var result = Factory.Create(definition);

            result.Method.Should().NotBeNull();
            result.Method.Should().Be(Options.Value.DefaultModelActivatorMethod);
            result.Factory.Should().BeNull();
            result.FactoryType.Should().BeNull();
        }

        [Fact]
        public void FactoryMethod_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.RequestFactoryDefinition = new RequestFactoryExpression(() => new Request());
            var result = Factory.Create(definition);

            result.Method.Should().NotBeNull();
            var request = result.Method(null) as Request;
            request.Should().BeOfType<Request>();
            result.Factory.Should().BeNull();
            result.FactoryType.Should().BeNull();
        }

        [Fact]
        public void FactoryType_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.RequestFactoryDefinition = new RequestFactoryExpression(typeof(TestFactory));
            var result = Factory.Create(definition);

            result.Method.Should().BeNull();
            result.Factory.Should().BeNull();
            result.FactoryType.Should().Be<TestFactory>();
        }

        [Fact]
        public void FactoryServiceProvider_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            definition.RequestFactoryDefinition = new RequestFactoryExpression(sp => new Request());
            var result = Factory.Create(definition);

            result.Method.Should().BeNull();
            result.Factory.Should().NotBeNull();
            result.Factory(serviceProvider, typeof(Request)).Should().BeOfType<Request>();
            result.FactoryType.Should().BeNull();
        }

        [Fact]
        public void Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.RequestFactoryDefinition = new Mock<IRequestFactoryDefinition>().Object;
            var result = Factory.Create(definition);

            result.Method.Should().Be(Options.Value.DefaultModelActivatorMethod);
            result.Factory.Should().BeNull();
            result.FactoryType.Should().BeNull();
        }

        private class TestFactory : IModelFactory
        {
            public object Create(Type modelType) => Activator.CreateInstance(modelType);
        }

        private class Item
        {
        }

        private class Request
        {
        }

        private class Result
        {
        }
    }
}