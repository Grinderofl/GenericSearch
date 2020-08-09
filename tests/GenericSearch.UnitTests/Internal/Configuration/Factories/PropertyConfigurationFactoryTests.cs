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
    public class PropertyConfigurationFactoryTests
    {
        private PropertyConfigurationFactory Factory => new PropertyConfigurationFactory();

        [Fact]
        public void Create_Property_Convention_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Property(x => x.Foo);
            var foo = typeof(Request).GetProperty(nameof(Request.Foo));
            var bar = typeof(Request).GetProperty(nameof(Request.Bar));

            var fooResult = Factory.Create(foo, definition);
            var barResult = Factory.Create(bar, definition);

            fooResult.RequestProperty.Name.Should().Be("Foo");
            fooResult.ResultProperty.Name.Should().Be("Foo");
            fooResult.Ignored.Should().BeFalse();
            fooResult.DefaultValue.Should().BeNull();

            barResult.RequestProperty.Name.Should().Be("Bar");
            barResult.ResultProperty.Name.Should().Be("Bar");
            barResult.Ignored.Should().BeFalse();
            barResult.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Convention_DefaultValueAttribute_Succeeds()
        {
            var definition = new ListExpression<Request2, Item, Result>();
            definition.Property(x => x.Foo);
            var foo = typeof(Request2).GetProperty(nameof(Request2.Foo));
            var bar = typeof(Request2).GetProperty(nameof(Request2.Bar));

            var fooResult = Factory.Create(foo, definition);
            var barResult = Factory.Create(bar, definition);

            fooResult.RequestProperty.Name.Should().Be("Foo");
            fooResult.ResultProperty.Name.Should().Be("Foo");
            fooResult.Ignored.Should().BeFalse();
            fooResult.DefaultValue.Should().Be("Text");

            barResult.RequestProperty.Name.Should().Be("Bar");
            barResult.ResultProperty.Name.Should().Be("Bar");
            barResult.Ignored.Should().BeFalse();
            barResult.DefaultValue.Should().Be(1);
        }

        [Fact]
        public void Create_Property_Convention_DefaultValue_Succeeds()
        {
            var definition = new ListExpression<Request, Item, Result>();
            definition.Property(x => x.Foo, x => x.DefaultValue("Text"));
            var foo = typeof(Request).GetProperty(nameof(Request.Foo));
            var bar = typeof(Request).GetProperty(nameof(Request.Bar));

            var fooResult = Factory.Create(foo, definition);
            var barResult = Factory.Create(bar, definition);

            fooResult.RequestProperty.Name.Should().Be("Foo");
            fooResult.ResultProperty.Name.Should().Be("Foo");
            fooResult.Ignored.Should().BeFalse();
            fooResult.DefaultValue.Should().Be("Text");

            barResult.RequestProperty.Name.Should().Be("Bar");
            barResult.ResultProperty.Name.Should().Be("Bar");
            barResult.Ignored.Should().BeFalse();
            barResult.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Ignored_Succeeds()
        {
            var definition = new ListExpression<Request3, Item, Result>();
            definition.Property(x => x.Foo, x => x.Ignore());
            var foo = typeof(Request3).GetProperty(nameof(Request3.Foo));
            var bar = typeof(Request3).GetProperty(nameof(Request3.Bar));

            var result = Factory.Create(foo, definition);
            
            result.RequestProperty.Name.Should().Be("Foo");
            result.ResultProperty.Name.Should().Be("Foo");
            result.Ignored.Should().BeTrue();
            result.DefaultValue.Should().Be(null);
        }

        [Fact]
        public void Create_Convention_Mismatch_Throws()
        {
            var definition = new ListExpression<Request3, Item, Result>();
            var foo = typeof(Request3).GetProperty(nameof(Request3.Foo));

            Factory.Invoking(x => x.Create(foo, definition))
                .Should()
                .ThrowExactly<PropertyTypeMismatchException>();
        }

        [Fact]
        public void Create_Convention_Nomatch_Succeeds()
        {
            var definition = new ListExpression<Request4, Item, Result>();
            var foo = typeof(Request4).GetProperty(nameof(Request4.Baz));

            var result = Factory.Create(foo, definition);

            result.Should().BeNull();
        }

        [Fact]
        public void Create_Property_Mismatch_Throws()
        {
            var definition = new ListExpression<Request3, Item, Result>();
            definition.Property(x => x.Bar);
            var foo = typeof(Request3).GetProperty(nameof(Request3.Bar));

            Factory.Invoking(x => x.Create(foo, definition))
                .Should()
                .ThrowExactly<PropertyTypeMismatchException>();
        }


        private class Item
        {
        }

        private class Request
        {
            public string Foo { get; set; }
            public int Bar { get; set; }
        }

        private class Request2
        {
            [DefaultValue("Text")]
            public string Foo { get; set; }

            [DefaultValue(1)]
            public int Bar { get; set; }
        }

        private class Request3
        {
            public int Foo { get; set; }

            [DefaultValue("Text")]
            public string Bar { get; set; }
        }

        private class Request4
        {
            public string Baz { get; set; }
        }

        private class Result
        {
            public string Foo { get; set; }
            public int Bar { get; set; }
        }
    }
}