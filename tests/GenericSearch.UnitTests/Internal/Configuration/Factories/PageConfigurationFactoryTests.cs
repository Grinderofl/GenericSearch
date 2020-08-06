using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition.Expressions;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class PageConfigurationFactoryTests : ConfigurationFactoryTestBase
    {
        private PageConfigurationFactory Factory => new PageConfigurationFactory(Options);
        
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

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_AllByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.PageDefinition = new PageExpression<Request, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NoDefinitionByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_NameByConvention_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.PageDefinition = new PageExpression<Request3, Result>();
            var configuration = Factory.Create(definition);


            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().Be("Page");
        }

        [Fact]
        public void Create_AllByConvention_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            definition.PageDefinition = new PageExpression<Request2, Result>();

            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            definition.PageDefinition = new PageExpression<Request, Result>(x => x.Page);
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapTo_Property_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new PageExpression<Request, Result2>(x => x.Page);
            expression.MapTo(x => x.CurrentPage);
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("CurrentPage");
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_MapToInvalidProperty_Throws()
        {
            var definition = TestListDefinition.Create<Request, Item, Result2>();
            var expression = new PageExpression<Request, Result2>(x => x.Page);
            definition.PageDefinition = expression;

            Factory.Invoking(x => x.Create(definition))
                .Should()
                .ThrowExactly<PropertyNotFoundException>();
        }

        [Fact]
        public void Create_Property_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new PageExpression<Request, Result>(x => x.Page);
            ((IPageExpression<Request, Result>) expression).DefaultValue(2);
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Property_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new PageExpression<Request2, Result>(x => x.Page);
            
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new PageExpression<Request, Result>("Page");
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var expression = new PageExpression<Request, Result>("Page");
            ((IPageExpression) expression).DefaultValue(2);
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_DefaultValueByAttribute_Succeeds()
        {
            var definition = TestListDefinition.Create<Request2, Item, Result>();
            var expression = new PageExpression<Request2, Result>("Page");
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Name.Should().Be("Page");
            configuration.ResultProperty.Name.Should().Be("Page");
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().BeNull();
        }

        [Fact]
        public void Create_Name_Unmatched_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            definition.PageDefinition = new PageExpression<Request3, Result>("Page");
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(1);
            configuration.Name.Should().Be("Page");
        }

        [Fact]
        public void Create_Name_Unmatched_DefaultValue_Succeeds()
        {
            var definition = TestListDefinition.Create<Request3, Item, Result>();
            var expression = new PageExpression<Request3, Result>("Page");
            ((IPageExpression) expression).DefaultValue(2);
            definition.PageDefinition = expression;
            
            var configuration = Factory.Create(definition);

            configuration.RequestProperty.Should().BeNull();
            configuration.ResultProperty.Should().BeNull();
            configuration.DefaultValue.Should().Be(2);
            configuration.Name.Should().Be("Page");
        }

        private class Item
        {
        }

        private class Request
        {
            public int Page { get; set; }
        }

        private class Result
        {
            public int Page { get; set; }
        }

        private class Request2
        {
            [DefaultValue(2)]
            public int Page { get; set; }
        }
        
        private class Request3
        {
            public int CurrentPage { get; set; }
        }

        private class Result2
        {
            public int CurrentPage { get; set; }
        }
    }
}