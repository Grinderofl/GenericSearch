using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using GenericSearch.Internal.Activation.Finders;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition.Expressions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Configuration.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class SearchConfigurationFactoryTests
    {
        private SearchConfigurationFactory Factory => new SearchConfigurationFactory(new PascalCasePropertyPathFinder());

        [Fact]
        public void Create_ByConvention_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_Property_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text);
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_Property_MapTo_Property_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.MapTo(m => m.Bar));
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Bar");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_Property_On_Property_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.On(i => i.Bar));
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Bar");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_ChildProperty_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.ChildBaz);
            var property = typeof(Request).GetProperty(nameof(Request.ChildBaz));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("ChildBaz");
            result.ItemPropertyPath.Should().Be("Child.Baz");
            result.ResultProperty.Name.Should().Be("ChildBaz");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_ChildProperty_On_Path_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.ChildBaz, x => x.On("Child.Baz"));
            var property = typeof(Request).GetProperty(nameof(Request.ChildBaz));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("ChildBaz");
            result.ItemPropertyPath.Should().Be("Child.Baz");
            result.ResultProperty.Name.Should().Be("ChildBaz");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_ChildProperty_On_Property_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.ChildBaz, x => x.On(c => c.Child.Baz));
            var property = typeof(Request).GetProperty(nameof(Request.ChildBaz));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("ChildBaz");
            result.ItemPropertyPath.Should().Be("Child.Baz");
            result.ResultProperty.Name.Should().Be("ChildBaz");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_Property_Ignored_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.Ignore());
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeTrue();
        }

        [Fact]
        public void Create_Property_ConstructUsing_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.ActivateUsing(() => new TextSearch()));
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().NotBeNull();
            result.Activator.Should().BeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();
        }

        [Fact]
        public void Create_Property_ActivateUsing_ServiceProvider_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.ActivateUsing(s => s.GetRequiredService<TextSearchActivator>()));
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().NotBeNull();
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();

            var provider = new ServiceCollection()
                .AddSingleton<TextSearchActivator>()
                .BuildServiceProvider();

            var instance = result.Activator.Invoke(provider);
            instance.Should().BeOfType<TextSearchActivator>();
        }

        [Fact]
        public void Create_Property_ActivateUsing_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.ActivateUsing<TextSearchActivator>());
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().NotBeNull();
            result.Activator.Should().BeOfType(typeof(Func<IServiceProvider, ISearchActivator>));
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();

            var provider = new ServiceCollection()
                .AddSingleton<TextSearchActivator>()
                .BuildServiceProvider();

            var instance = result.Activator.Invoke(provider);
            instance.Should().BeOfType<TextSearchActivator>();
        }

        [Fact]
        public void Create_Property_ActivateUsing_Type_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Search(x => x.Text, x => x.ActivateUsing(typeof(TextSearchActivator)));
            var property = typeof(Request).GetProperty(nameof(Request.Text));

            var result = Factory.Create(property, definition);

            result.Constructor.Should().BeNull();
            result.Activator.Should().NotBeNull();
            result.Activator.Should().BeOfType(typeof(Func<IServiceProvider, ISearchActivator>));
            result.RequestProperty.Name.Should().Be("Text");
            result.ItemPropertyPath.Should().Be("Text");
            result.ResultProperty.Name.Should().Be("Text");
            result.Ignored.Should().BeFalse();

            var provider = new ServiceCollection()
                .AddSingleton<TextSearchActivator>()
                .BuildServiceProvider();

            var instance = result.Activator.Invoke(provider);
            instance.Should().BeOfType<TextSearchActivator>();
        }

        private class Item
        {
            public string Text { get; set; }
            public string Bar { get; set; }
            public SubItem Child { get; set; }
        }

        private class SubItem
        {
            public string Baz { get; set; }
        }

        private class Request
        {
            public TextSearch Text { get; set; }
            public TextSearch ChildBaz { get; set; }
        }

        private class Result
        {
            public TextSearch Text { get; set; }
            public TextSearch Bar { get; set; }
            public TextSearch ChildBaz { get; set; }
        }
    }
}