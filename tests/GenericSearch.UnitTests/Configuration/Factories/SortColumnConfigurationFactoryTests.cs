using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Configuration.Factories;
using GenericSearch.Definition.Expressions;
using GenericSearch.Exceptions;
using Xunit;

namespace GenericSearch.UnitTests.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class SortColumnConfigurationFactoryTests : ConfigurationFactoryTestBase
    {
        private SortColumnConfigurationFactory Factory => new SortColumnConfigurationFactory(Options);

        [Fact]
        public void Create_NoDefinition_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            var configuration = Factory.Create(definition);
            configuration.Should().BeNull();
        }

        
        [Fact]
        public void Create_NoDefinitionByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_AllByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.SortColumnDefinition = new SortColumnExpression<Request, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NoDefinitionByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NameByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.SortColumnDefinition = new SortColumnExpression<Request3, Item, Result>();
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().Be("Ordx");
        }

        [Fact]
        public void Create_AllByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            definition.SortColumnDefinition = new SortColumnExpression<Request2, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.SortColumnDefinition = new SortColumnExpression<Request, Item, Result>(x => x.Ordx);
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapTo_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new SortColumnExpression<Request, Item, Result2>(x => x.Ordx);
            expression.MapTo(x => x.Sortx);
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Sortx");
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapToInvalidProperty_Throws()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new SortColumnExpression<Request, Item, Result2>(x => x.Ordx);
            definition.SortColumnDefinition = expression;

            Factory.Invoking(x => x.Create(definition))
                .Should()
                .ThrowExactly<PropertyNotFoundException>();
        }

        [Fact]
        public void Create_Property_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new SortColumnExpression<Request, Item, Result>(x => x.Ordx);
            ((ISortColumnExpression<Request, Item, Result>) expression).DefaultValue("Text");
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new SortColumnExpression<Request2, Item, Result>(x => x.Ordx);
            
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new SortColumnExpression<Request, Item, Result>("Ordx");
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new SortColumnExpression<Request, Item, Result>("Ordx");
            ((ISortColumnExpression<Request, Item, Result>) expression).DefaultValue("Text");
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new SortColumnExpression<Request2, Item, Result>("Ordx");
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordx");
            configuration.ResultProperty.Name.Should().Be("Ordx");
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Unmatched_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.SortColumnDefinition = new SortColumnExpression<Request3, Item, Result>("Ordx");
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().BeNull();
            configuration.Name.Should().Be("Ordx");
        }

        [Fact]
        public void Create_Name_Unmatched_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            var expression = new SortColumnExpression<Request3, Item, Result>("Ordx");
            ((ISortColumnExpression<Request3, Item, Result>) expression).DefaultValue("Text");
            definition.SortColumnDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be("Text");
            configuration.Name.Should().Be("Ordx");
        }

        private class Item
        {
            public string Text { get; set; }
        }

        private class Request
        {
            public string Ordx { get; set; }
        }

        private class Result
        {
            public string Ordx { get; set; }
        }

        private class Request2
        {
            [DefaultValue("Text")]
            public string Ordx { get; set; }
        }
        
        private class Request3
        {
            public string Sortx { get; set; }
        }

        private class Result2
        {
            public string Sortx { get; set; }
        }
    }
}