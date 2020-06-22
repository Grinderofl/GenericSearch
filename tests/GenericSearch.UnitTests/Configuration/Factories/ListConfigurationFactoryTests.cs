using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Configuration.Factories;
using GenericSearch.Definition.Expressions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ListConfigurationFactoryTests
    {
        public ListConfigurationFactoryTests()
        {
            var options = new GenericSearchOptions();

            var optionsMock = new Mock<IOptions<GenericSearchOptions>>();
            optionsMock.SetupGet(x => x.Value).Returns(options);

            var filterConfigurationFactory = new SearchConfigurationFactory(new PascalCasePropertyPathFinder());
            var pageConfigurationFactory = new PageConfigurationFactory(optionsMock.Object);
            var rowsConfigurationFactory = new RowsConfigurationFactory(optionsMock.Object);
            var sortColumnConfigurationFactory = new SortColumnConfigurationFactory(optionsMock.Object);
            var sortDirectionConfigurationFactory = new SortDirectionConfigurationFactory(optionsMock.Object);
            var propertyConfigurationFactory = new PropertyConfigurationFactory();
            var postRedirectGetConfigurationFactory = new PostRedirectGetConfigurationFactory(optionsMock.Object);
            var transferValuesConfigurationFactory = new TransferValuesConfigurationFactory(optionsMock.Object);
            var requestFactoryConfigurationFactory = new RequestFactoryConfigurationFactory(optionsMock.Object);
            factory = new ListConfigurationFactory(filterConfigurationFactory,
                                                   pageConfigurationFactory,
                                                   rowsConfigurationFactory,
                                                   sortColumnConfigurationFactory,
                                                   sortDirectionConfigurationFactory,
                                                   propertyConfigurationFactory,
                                                   postRedirectGetConfigurationFactory,
                                                   transferValuesConfigurationFactory,
                                                   requestFactoryConfigurationFactory);
        }

        private readonly ListConfigurationFactory factory;

        [Fact]
        public void Create_AllByConvention_Succeeds()
        {
            var source = new ListExpression<Request, Item, Result>();
            
            var configuration = factory.Create(source);
            
            configuration.RequestType.Should().Be<Request>();
            configuration.ItemType.Should().Be<Item>();
            configuration.ResultType.Should().Be<Result>();
            
            configuration.ResultPropertyFor("Text").Name.Should().Be("Text");
            configuration.ItemPropertyPathFor("Text").Should().Be("Text");
            configuration.ResultPropertyFor("Integer").Name.Should().Be("Integer");
            configuration.ItemPropertyPathFor("Integer").Should().Be("Integer");
            
            configuration.PageConfiguration.RequestProperty.Name.Should().Be("Page");
            configuration.PageConfiguration.ResultProperty.Name.Should().Be("Page");
            configuration.PageConfiguration.DefaultValue.Should().Be(1);

            configuration.RowsConfiguration.RequestProperty.Name.Should().Be("Rows");
            configuration.RowsConfiguration.ResultProperty.Name.Should().Be("Rows");
            configuration.RowsConfiguration.DefaultValue.Should().Be(25);

            configuration.SortColumnConfiguration.RequestProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.ResultProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.DefaultValue.Should().BeNull();

            configuration.SortDirectionConfiguration.RequestProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.ResultProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.DefaultValue.Should().Be(Direction.Ascending);

            var property = configuration.PropertyConfigurations.Single(x => x.RequestProperty.Name.Equals("Query"));
            property.ResultProperty.Name.Should().Be("Query");
            property.Ignored.Should().BeFalse();

            configuration.PostRedirectGetConfiguration.ActionName.Should().Be("Index");
            configuration.PostRedirectGetConfiguration.Enabled.Should().BeTrue();

            configuration.TransferValuesConfiguration.ActionName.Should().Be("Index");
            configuration.TransferValuesConfiguration.Enabled.Should().BeTrue();

            configuration.RequestFactoryConfiguration.Should().NotBeNull();
        }

        [Fact]
        public void Create_SearchesBySelector_PageRowsPropertiesByConvention_Succeeds()
        {
            var source = new ListExpression<Request, Item, DifferentFiltersResult>();
            source.Search(x => x.Text, x => x.MapTo(m => m.Integer))
                .Search(x => x.Integer, x => x.MapTo(m => m.Text));

            var configuration = factory.Create(source);

            configuration.RequestType.Should().Be<Request>();
            configuration.ItemType.Should().Be<Item>();
            configuration.ResultType.Should().Be<DifferentFiltersResult>();
            
            configuration.ResultPropertyFor("Text").Name.Should().Be("Integer");
            configuration.ItemPropertyPathFor("Text").Should().Be("Text");
            configuration.ResultPropertyFor("Integer").Name.Should().Be("Text");
            configuration.ItemPropertyPathFor("Integer").Should().Be("Integer");

            configuration.PageConfiguration.RequestProperty.Name.Should().Be("Page");
            configuration.PageConfiguration.ResultProperty.Name.Should().Be("Page");
            configuration.PageConfiguration.DefaultValue.Should().Be(1);

            configuration.RowsConfiguration.RequestProperty.Name.Should().Be("Rows");
            configuration.RowsConfiguration.ResultProperty.Name.Should().Be("Rows");
            configuration.RowsConfiguration.DefaultValue.Should().Be(25);

            configuration.SortColumnConfiguration.RequestProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.ResultProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.DefaultValue.Should().BeNull();

            configuration.SortDirectionConfiguration.RequestProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.ResultProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.DefaultValue.Should().Be(Direction.Ascending);

            var property = configuration.PropertyConfigurations.Single(x => x.RequestProperty.Name.Equals("Query"));
            property.ResultProperty.Name.Should().Be("Query");
            property.Ignored.Should().BeFalse();
            
            configuration.PostRedirectGetConfiguration.ActionName.Should().Be("Index");
            configuration.PostRedirectGetConfiguration.Enabled.Should().BeTrue();
            
            configuration.TransferValuesConfiguration.ActionName.Should().Be("Index");
            configuration.TransferValuesConfiguration.Enabled.Should().BeTrue();

            configuration.RequestFactoryConfiguration.Should().NotBeNull();
        }

        [Fact]
        public void Create_SearchesPropertiesByConvention_PageRowsBySelector_Succeeds()
        {
            var source = new ListExpression<Request, Item, DifferentPageAndRowsResult>();
            source.Page(x => x.Page, x => x.MapTo(m => m.CurrentPage).DefaultValue(2))
                .Rows(x => x.Rows, x => x.MapTo(m => m.CurrentRows).DefaultValue(1000));

            var configuration = factory.Create(source);

            configuration.RequestType.Should().Be<Request>();
            configuration.ItemType.Should().Be<Item>();
            configuration.ResultType.Should().Be<DifferentPageAndRowsResult>();
            
            configuration.ResultPropertyFor("Text").Name.Should().Be("Text");
            configuration.ItemPropertyPathFor("Text").Should().Be("Text");
            configuration.ResultPropertyFor("Integer").Name.Should().Be("Integer");
            configuration.ItemPropertyPathFor("Integer").Should().Be("Integer");

            configuration.PageConfiguration.RequestProperty.Name.Should().Be("Page");
            configuration.PageConfiguration.ResultProperty.Name.Should().Be("CurrentPage");
            configuration.PageConfiguration.DefaultValue.Should().Be(2);

            configuration.RowsConfiguration.RequestProperty.Name.Should().Be("Rows");
            configuration.RowsConfiguration.ResultProperty.Name.Should().Be("CurrentRows");
            configuration.RowsConfiguration.DefaultValue.Should().Be(1000);

            configuration.SortColumnConfiguration.RequestProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.ResultProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.DefaultValue.Should().BeNull();

            configuration.SortDirectionConfiguration.RequestProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.ResultProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.DefaultValue.Should().Be(Direction.Ascending);

            var property = configuration.PropertyConfigurations.Single(x => x.RequestProperty.Name.Equals("Query"));
            property.ResultProperty.Name.Should().Be("Query");
            property.Ignored.Should().BeFalse();
            
            configuration.PostRedirectGetConfiguration.ActionName.Should().Be("Index");
            configuration.PostRedirectGetConfiguration.Enabled.Should().BeTrue();
            
            configuration.TransferValuesConfiguration.ActionName.Should().Be("Index");
            configuration.TransferValuesConfiguration.Enabled.Should().BeTrue();

            configuration.RequestFactoryConfiguration.Should().NotBeNull();
        }

        [Fact]
        public void Create_All_BySelector_Succeeds()
        {
            var source = new ListExpression<Request, Item, DifferentPageAndRowsResult>();
            source.Page(x => x.Page, x => x.MapTo(m => m.CurrentPage).DefaultValue(2))
                .Rows(x => x.Rows, x => x.MapTo(m => m.CurrentRows).DefaultValue(1000));

            var configuration = factory.Create(source);

            configuration.RequestType.Should().Be<Request>();
            configuration.ItemType.Should().Be<Item>();
            configuration.ResultType.Should().Be<DifferentPageAndRowsResult>();
            
            configuration.ResultPropertyFor("Text").Name.Should().Be("Text");
            configuration.ItemPropertyPathFor("Text").Should().Be("Text");
            configuration.ResultPropertyFor("Integer").Name.Should().Be("Integer");
            configuration.ItemPropertyPathFor("Integer").Should().Be("Integer");

            configuration.PageConfiguration.RequestProperty.Name.Should().Be("Page");
            configuration.PageConfiguration.ResultProperty.Name.Should().Be("CurrentPage");
            configuration.PageConfiguration.DefaultValue.Should().Be(2);

            configuration.RowsConfiguration.RequestProperty.Name.Should().Be("Rows");
            configuration.RowsConfiguration.ResultProperty.Name.Should().Be("CurrentRows");
            configuration.RowsConfiguration.DefaultValue.Should().Be(1000);

            configuration.SortColumnConfiguration.RequestProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.ResultProperty.Name.Should().Be("Ordx");
            configuration.SortColumnConfiguration.DefaultValue.Should().BeNull();

            configuration.SortDirectionConfiguration.RequestProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.ResultProperty.Name.Should().Be("Ordd");
            configuration.SortDirectionConfiguration.DefaultValue.Should().Be(Direction.Ascending);

            var property = configuration.PropertyConfigurations.Single(x => x.RequestProperty.Name.Equals("Query"));
            property.ResultProperty.Name.Should().Be("Query");
            property.Ignored.Should().BeFalse();
            
            configuration.PostRedirectGetConfiguration.ActionName.Should().Be("Index");
            configuration.PostRedirectGetConfiguration.Enabled.Should().BeTrue();
            
            configuration.TransferValuesConfiguration.ActionName.Should().Be("Index");
            configuration.TransferValuesConfiguration.Enabled.Should().BeTrue();

            configuration.RequestFactoryConfiguration.Should().NotBeNull();
        }
        
        private class Item
        {
            public string Text { get; set; }
            public int Integer { get; set; }
        }

        private class Request
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }
            public string NotAQuery { get; set; }
        }

        private class Result
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }
        }

        private class DifferentFiltersResult
        {
            public TextSearch Integer { get; set; }
            public IntegerSearch Text { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }
        }

        private class DifferentPageAndRowsResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int CurrentPage { get; set; }
            public int CurrentRows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }
        }
        private class DifferentQueryResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int CurrentPage { get; set; }
            public int CurrentRows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }
        }

        private class DifferentSortResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Sortx { get; set; }
            public Direction Sortd { get; set; }
            public string Query { get; set; }
        }
        
        private class MissingFiltersResult
        {
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Query { get; set; }
        }

        private class MissingPageResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public string Query { get; set; }
        }
        private class MissingQueryResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
        }


        private class DefaultPageResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }

            [DefaultValue(2)]
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Query { get; set; }
        }

        private class DefaultPageZeroResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }

            [DefaultValue(0)]
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Query { get; set; }
        }

        private class DefaultRowsResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int Page { get; set; }

            [DefaultValue(1000)]
            public int Rows { get; set; }
        }

        private class DefaultRowsZeroResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public int Page { get; set; }

            [DefaultValue(0)]
            public int Rows { get; set; }
        }
    }
}