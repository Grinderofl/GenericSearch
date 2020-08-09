using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition;
using GenericSearch.Internal.Definition.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ModelActivatorConfigurationFactoryTests : ConfigurationFactoryTestBase
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
            definition.ModelActivatorDefinition = new ModelActivatorExpression(_ => new Request());
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
            definition.ModelActivatorDefinition = new ModelActivatorExpression(typeof(TestFactory));
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
            definition.ModelActivatorDefinition = new ModelActivatorExpression((sp, _) => new Request());
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
            definition.ModelActivatorDefinition = new Mock<IModelActivatorDefinition>().Object;
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