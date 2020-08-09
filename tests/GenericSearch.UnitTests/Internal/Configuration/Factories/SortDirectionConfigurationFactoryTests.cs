using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition.Expressions;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class SortDirectionConfigurationFactoryTests : ConfigurationFactoryTestBase
    {
        private SortDirectionConfigurationFactory Factory => new SortDirectionConfigurationFactory(Options);

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

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_AllByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.SortDirectionDefinition = new SortDirectionExpression<Request, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NoDefinitionByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NameByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.SortDirectionDefinition = new SortDirectionExpression<Request3, Result>();
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().Be("Ordd");
        }

        [Fact]
        public void Create_AllByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            definition.SortDirectionDefinition = new SortDirectionExpression<Request2, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.SortDirectionDefinition = new SortDirectionExpression<Request, Result>(x => x.Ordd);
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapTo_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new SortDirectionExpression<Request, Result2>(x => x.Ordd);
            expression.MapTo(x => x.Sortd);
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Sortd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapToInvalidProperty_Throws()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new SortDirectionExpression<Request, Result2>(x => x.Ordd);
            definition.SortDirectionDefinition = expression;

            Factory.Invoking(x => x.Create(definition))
                .Should()
                .ThrowExactly<PropertyNotFoundException>();
        }

        [Fact]
        public void Create_Property_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new SortDirectionExpression<Request, Result>(x => x.Ordd);
            ((ISortDirectionExpression<Request, Result>) expression).DefaultValue(Direction.Ascending);
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new SortDirectionExpression<Request2, Result>(x => x.Ordd);
            
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new SortDirectionExpression<Request, Result>("Ordd");
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new SortDirectionExpression<Request, Result>("Ordd");
            ((ISortDirectionExpression<Request, Result>) expression).DefaultValue(Direction.Ascending);
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new SortDirectionExpression<Request2, Result>("Ordd");
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Ordd");
            configuration.ResultProperty.Name.Should().Be("Ordd");
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Unmatched_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.SortDirectionDefinition = new SortDirectionExpression<Request3, Result>("Ordd");
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().Be("Ordd");
        }

        [Fact]
        public void Create_Name_Unmatched_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            var expression = new SortDirectionExpression<Request3, Result>("Ordd");
            ((ISortDirectionExpression<Request3, Result>) expression).DefaultValue(Direction.Ascending);
            definition.SortDirectionDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(Direction.Ascending);
            configuration.Name.Should().Be("Ordd");
        }

        private class Item
        {
            public string Text { get; set; }
        }

        private class Request
        {
            public Direction Ordd { get; set; }
        }

        private class Result
        {
            public Direction Ordd { get; set; }
        }

        private class Request2
        {
            [DefaultValue(Direction.Ascending)]
            public Direction Ordd { get; set; }
        }
        
        private class Request3
        {
            public Direction Sortd { get; set; }
        }

        private class Result2
        {
            public Direction Sortd { get; set; }
        }
    }
}