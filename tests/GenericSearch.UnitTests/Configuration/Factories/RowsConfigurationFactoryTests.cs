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
    public class RowsConfigurationFactoryTests : ConfigurationFactoryTestBase
    {
        private RowsConfigurationFactory Factory => new RowsConfigurationFactory(Options);

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

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().BeNull();
        }





        
        [Fact]
        public void Create_AllByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.RowsDefinition = new RowsExpression<Request, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NoDefinitionByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(30);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NameByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.RowsDefinition = new RowsExpression<Request3, Result>();
            var configuration = Factory.Create(definition);


            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().Be("Rows");
        }

        [Fact]
        public void Create_AllByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            definition.RowsDefinition = new RowsExpression<Request2, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(30);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.RowsDefinition = new RowsExpression<Request, Result>(x => x.Rows);
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapTo_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new RowsExpression<Request, Result2>(x => x.Rows);
            expression.MapTo(x => x.CurrentRows);
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("CurrentRows");
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapToInvalidProperty_Throws()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new RowsExpression<Request, Result2>(x => x.Rows);
            definition.RowsDefinition = expression;

            Factory.Invoking(x => x.Create(definition))
                .Should()
                .ThrowExactly<PropertyNotFoundException>();
        }

        [Fact]
        public void Create_Property_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new RowsExpression<Request, Result>(x => x.Rows);
            ((IRowsExpression<Request, Result>) expression).DefaultValue(30);
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(30);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new RowsExpression<Request2, Result>(x => x.Rows);
            
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(30);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new RowsExpression<Request, Result>("Rows");
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new RowsExpression<Request, Result>("Rows");
            ((IRowsExpression) expression).DefaultValue(2);
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new RowsExpression<Request2, Result>("Rows");
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Rows");
            configuration.ResultProperty.Name.Should().Be("Rows");
            configuration.DefaultValue.Should().Be(30);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Unmatched_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.RowsDefinition = new RowsExpression<Request3, Result>("Rows");
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(25);
            configuration.Name.Should().Be("Rows");
        }

        [Fact]
        public void Create_Name_Unmatched_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            var expression = new RowsExpression<Request3, Result>("Rows");
            ((IRowsExpression) expression).DefaultValue(2);
            definition.RowsDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().Be("Rows");
        }

        private class Item
        {
        }

        private class Request
        {
            public int Rows { get; set; }
        }

        private class Result
        {
            public int Rows { get; set; }
        }

        private class Request2
        {
            [DefaultValue(30)]
            public int Rows { get; set; }
        }
        
        private class Request3
        {
            public int CurrentRows { get; set; }
        }

        private class Result2
        {
            public int CurrentRows { get; set; }
        }
    }
}