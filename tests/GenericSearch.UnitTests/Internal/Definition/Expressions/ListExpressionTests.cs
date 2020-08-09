using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Internal.Definition.Expressions;
using GenericSearch.Searches;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Definition.Expressions
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ListExpressionTests
    {
        [Fact]
        public void Search_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text);

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().BeNull();
        }

        [Fact]
        public void Search_Property_MapTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.MapTo(m => m.Text));

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Name.Should().Be("Text");
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().BeNull();
        }

        [Fact]
        public void Search_Property_On_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.On(m => m.Text));

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().Be("Text");
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().BeNull();
        }

        [Fact]
        public void Search_Property_Ignore_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.Ignore());

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeTrue();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().BeNull();
        }

        [Fact]
        public void Search_Property_ConstructUsing_FactoryMethod_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.ActivateUsing(() => new TextSearch()));

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().NotBeNull();
            definition.Value.Activator.Should().BeNull();
        }

        [Fact]
        public void Search_Property_ActivateUsing_ServiceProvider_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.ActivateUsing(s => s.GetService<ISearchActivator<TextSearch>>()));

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().NotBeNull();
            definition.Value.ActivatorType.Should().BeNull();
        }

        [Fact]
        public void Search_Property_ActivateUsing_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.ActivateUsing<ISearchActivator<TextSearch>>());

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().BeNull();
            definition.Value.ActivatorType.Should().NotBeNull();
        }

        [Fact]
        public void Search_Property_ActivateUsing_Type_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Search(x => x.Text, x => x.ActivateUsing(typeof(ISearchActivator<TextSearch>)));

            var definition = expression.SearchDefinitions.First();
            definition.Key.Name.Should().Be("Text");
            definition.Value.RequestProperty.Name.Should().Be("Text");
            definition.Value.ItemPropertyPath.Should().BeNull();
            definition.Value.ResultProperty.Should().BeNull();
            definition.Value.Ignored.Should().BeFalse();
            definition.Value.Constructor.Should().BeNull();
            definition.Value.Activator.Should().BeNull();
            definition.Value.ActivatorType.Should().NotBeNull();
        }

        [Fact]
        public void Page_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page(x => x.Page);
            var definition = expression.PageDefinition;

            definition.RequestProperty.Name.Should().Be("Page");
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().BeNull();
        }


        [Fact]
        public void Page_Property_MapTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page(x => x.Page, x => x.MapTo(m => m.Page));
            var definition = expression.PageDefinition;

            definition.RequestProperty.Name.Should().Be("Page");
            definition.ResultProperty.Name.Should().Be("Page");
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void Page_Property_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page(x => x.Page, x => x.DefaultValue(2));
            var definition = expression.PageDefinition;

            definition.RequestProperty.Name.Should().Be("Page");
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().Be(2);
        }

        [Fact]
        public void Page_Property_DefaultValueByAttribute_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page(x => x.CurrentPage);
            var definition = expression.PageDefinition;

            definition.RequestProperty.Name.Should().Be("CurrentPage");
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().Be(2);
        }

        [Fact]
        public void Page_Name_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page("Page");
            var definition = expression.PageDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().Be("Page");
            definition.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void Page_Name_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page("Page", x => x.DefaultValue(2));
            var definition = expression.PageDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().Be("Page");
            definition.DefaultValue.Should().Be(2);
        }

        [Fact]
        public void Page_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page(2);
            var definition = expression.PageDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().Be(2);
        }


        [Fact]
        public void Page_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Page();
            var definition = expression.PageDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void Rows_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows(x => x.Rows);
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Name.Should().Be("Rows");
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().BeNull();
        }


        [Fact]
        public void Rows_Property_MapTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows(x => x.Rows, x => x.MapTo(m => m.Rows));
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Name.Should().Be("Rows");
            definition.ResultProperty.Name.Should().Be("Rows");
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void Rows_Property_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows(x => x.Rows, x => x.DefaultValue(2));
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Name.Should().Be("Rows");
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().Be(2);
        }

        [Fact]
        public void Rows_Property_DefaultValueByAttribute_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows(x => x.CurrentRows);
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Name.Should().Be("CurrentRows");
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().Be(2);
        }


        [Fact]
        public void Rows_Name_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows("Rows");
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().Be("Rows");
            definition.DefaultValue.Should().BeNull();
        }


        [Fact]
        public void Rows_Name_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows("Rows", x => x.DefaultValue(2));
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().Be("Rows");
            definition.DefaultValue.Should().Be(2);
        }


        [Fact]
        public void Rows_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows(2);
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().Be(2);
        }


        [Fact]
        public void Rows_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Rows();
            var definition = expression.RowsDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.Name.Should().BeNullOrWhiteSpace();
            definition.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void SortColumn_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn(x => x.Ordx);
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Name.Should().Be("Ordx");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortColumn_Property_MapTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn(x => x.Ordx, x => x.MapTo(m => m.Ordx));
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Name.Should().Be("Ordx");
            definition.ResultProperty.Name.Should().Be("Ordx");
            definition.DefaultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }


        [Fact]
        public void SortColumn_Property_DefaultTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn(x => x.Ordx, x => x.DefaultTo(m => m.Text));
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Name.Should().Be("Ordx");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Name.Should().Be("Text");
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortColumn_Property_DefaultValueByAttribute_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn(x => x.Sortx);
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Name.Should().Be("Sortx");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be("Text");
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortColumn_Name_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn("Sortx");

            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().Be("Sortx");
        }

        [Fact]
        public void SortColumn_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn("", x => x.DefaultValue("Text"));
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be("Text");
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortColumn_DefaultValue_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn("", x => x.DefaultTo(s => s.Text));
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Name.Should().Be("Text");
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortColumn_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortColumn();
            var definition = expression.SortColumnDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void SortDirection_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection(x => x.Ordd);
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Name.Should().Be("Ordd");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortDirection_Property_MapTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection(x => x.Ordd, x => x.MapTo(m => m.Ordd));
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Name.Should().Be("Ordd");
            definition.ResultProperty.Name.Should().Be("Ordd");
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortDirection_Property_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection(x => x.Ordd, x => x.DefaultValue(Direction.Ascending));
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Name.Should().Be("Ordd");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be(Direction.Ascending);
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortDirection_Property_DefaultValueByAttribute_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection(x => x.Sortd);
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Name.Should().Be("Sortd");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be(Direction.Ascending);
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortDirection_Name_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection("Direction");
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().Be("Direction");
        }

        [Fact]
        public void SortDirection_Name_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection("Direction", x => x.DefaultValue(Direction.Ascending));
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be(Direction.Ascending);
            definition.Name.Should().Be("Direction");
        }

        [Fact]
        public void SortDirection_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection("", x => x.DefaultValue(Direction.Ascending));
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be(Direction.Ascending);
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void SortDirection_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.SortDirection();
            var definition = expression.SortDirectionDefinition;

            definition.RequestProperty.Should().BeNull();
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Name.Should().BeNull();
        }

        [Fact]
        public void Property_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Property(x => x.Query);
            var definition = expression.PropertyDefinitions.First(x => x.Key.Name == "Query").Value;

            definition.RequestProperty.Name.Should().Be("Query");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Ignore.Should().BeNull();
        }

        [Fact]
        public void Property_Property_MapTo_Property_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Property(x => x.Query, x => x.MapTo(m => m.Query));
            var definition = expression.PropertyDefinitions.First(x => x.Key.Name == "Query").Value;

            definition.RequestProperty.Name.Should().Be("Query");
            definition.ResultProperty.Name.Should().Be("Query");
            definition.DefaultValue.Should().BeNull();
            definition.Ignore.Should().BeNull();
        }

        [Fact]
        public void Property_Property_DefaultValue_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Property(x => x.Query, x => x.DefaultValue("Test"));
            var definition = expression.PropertyDefinitions.First(x => x.Key.Name == "Query").Value;

            definition.RequestProperty.Name.Should().Be("Query");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be("Test");
            definition.Ignore.Should().BeNull();
        }

        [Fact]
        public void Property_Property_DefaultValueByAttribute_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Property(x => x.NotAQuery);
            var definition = expression.PropertyDefinitions.First().Value;

            definition.RequestProperty.Name.Should().Be("NotAQuery");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().Be("Test");
            definition.Ignore.Should().BeNull();
        }

        [Fact]
        public void Property_Property_Ignore_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.Property(x => x.Query, x => x.Ignore());
            var definition = expression.PropertyDefinitions.First(x => x.Key.Name == "Query").Value;

            definition.RequestProperty.Name.Should().Be("Query");
            definition.ResultProperty.Should().BeNull();
            definition.DefaultValue.Should().BeNull();
            definition.Ignore.Should().BeTrue();
        }

        [Fact]
        public void PostRedirectGet_ActionName_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.PostRedirectGet(x => x.UseActionName("List"));

            expression.PostRedirectGetDefinition.ActionName.Should().Be("List");
            expression.PostRedirectGetDefinition.Enabled.Should().BeNull();
        }

        [Fact]
        public void PostRedirectGet_Enabled_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.PostRedirectGet(x => x.Enable());

            expression.PostRedirectGetDefinition.ActionName.Should().BeNull();
            expression.PostRedirectGetDefinition.Enabled.Should().BeTrue();
        }

        [Fact]
        public void PostRedirectGet_Disabled_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.PostRedirectGet(x => x.Disable());

            expression.PostRedirectGetDefinition.ActionName.Should().BeNull();
            expression.PostRedirectGetDefinition.Enabled.Should().BeFalse();
        }

        [Fact]
        public void TransferValues_ActionName_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.TransferValues(x => x.UseActionName("List"));

            expression.TransferValuesDefinition.ActionName.Should().Be("List");
            expression.TransferValuesDefinition.Enabled.Should().BeNull();
        }

        [Fact]
        public void TransferValues_Enable_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.TransferValues(x => x.Enable());

            expression.TransferValuesDefinition.ActionName.Should().BeNull();
            expression.TransferValuesDefinition.Enabled.Should().BeTrue();
        }

        [Fact]
        public void TransferValues_Disable_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.TransferValues(x => x.Disable());

            expression.TransferValuesDefinition.ActionName.Should().BeNull();
            expression.TransferValuesDefinition.Enabled.Should().BeFalse();
        }

        [Fact]
        public void ConstructUsing_FactoryMethod_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.ConstructUsing(() => new Request());

            expression.ModelActivatorDefinition.Method.Should().NotBeNull();
            expression.ModelActivatorDefinition.FactoryType.Should().BeNull();
            expression.ModelActivatorDefinition.Factory.Should().BeNull();
            expression.ModelActivatorDefinition.Method(typeof(Request)).Should().BeOfType<Request>();
        }

        [Fact]
        public void ConstructUsing_ServiceProvider_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.ConstructUsing(sp => sp.GetRequiredService<Request>());
            var sp = new ServiceCollection().AddSingleton<Request>().BuildServiceProvider();

            expression.ModelActivatorDefinition.Method.Should().BeNull();
            expression.ModelActivatorDefinition.FactoryType.Should().BeNull();
            expression.ModelActivatorDefinition.Factory.Should().NotBeNull();

            expression.ModelActivatorDefinition.Factory(sp, typeof(Request)).Should().BeOfType<Request>();
        }

        [Fact]
        public void ConstructUsing_Factory_Succeeds()
        {
            var expression = new ListExpression<Request, Item, Result>();
            expression.ConstructUsing<TestFactory>();

            expression.ModelActivatorDefinition.Method.Should().BeNull();
            expression.ModelActivatorDefinition.Factory.Should().BeNull();
            expression.ModelActivatorDefinition.FactoryType.Should().Be<TestFactory>();
        }

        private class TestFactory : IModelFactory
        {
            public object Create(Type modelType)
            {
                return Activator.CreateInstance(modelType);
            }
        }

        private class Item
        {
            public string Text { get; set; }
        }

        private class Request
        {
            public TextSearch Text { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }

            [DefaultValue("Test")]
            public string NotAQuery { get; set; }

            [DefaultValue(2)]
            public int CurrentPage { get; set; }

            [DefaultValue(2)]
            public int CurrentRows { get; set; }

            [DefaultValue("Text")]
            public string Sortx { get; set; }

            [DefaultValue(Direction.Ascending)]
            public Direction Sortd { get; set; }
        }

        private class Result
        {
            public TextSearch Text { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public string Query { get; set; }
            public int CurrentPage { get; set; }
            public int CurrentRows { get; set; }
            public string Sortx { get; set; }
            public Direction Sortd { get; set; }
        }
    }
}