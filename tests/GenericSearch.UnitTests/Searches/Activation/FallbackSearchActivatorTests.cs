using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using Xunit;

namespace GenericSearch.UnitTests.Searches.Activation
{
    public class FallbackSearchActivatorTests
    {
        private static readonly string ItemPropertyPath = typeof(Item).GetProperty(nameof(Item.Property)).Name;

        [Fact]
        public void Private_Constructor_Throws()
        {
            var activator = new FallbackSearchActivator(typeof(PrivateConstructorSearch));

            activator.Invoking(x => x.Activate(ItemPropertyPath))
                .Should()
                .ThrowExactly<SearchPropertyActivationException>();
        }

        [Fact]
        public void String_Constructor_Succeeds()
        {
            var activator = new FallbackSearchActivator(typeof(StringConstructorSearch));

            var result = activator.Activate(ItemPropertyPath);

            result.Should().NotBeNull();
            result.Should().BeOfType<StringConstructorSearch>();
        }

        [Fact]
        public void Multiple_Constructors_Succeeds()
        {
            var activator = new FallbackSearchActivator(typeof(MultipleConstructorsSearch));

            var result = activator.Activate(ItemPropertyPath);

            result.Should().NotBeNull();
            result.Should().BeOfType<MultipleConstructorsSearch>();
        }

        [Fact]
        public void MultipleParameter_Constructor_Throws()
        {
            var activator = new FallbackSearchActivator(typeof(MultipleParameterConstructorSearch));

            activator.Invoking(x => x.Activate(ItemPropertyPath))
                .Should()
                .ThrowExactly<SearchPropertyActivationException>();
        }

        [Fact]
        public void No_Constructor_Succeeds()
        {
            var activator = new FallbackSearchActivator(typeof(NoConstructorSearch));

            var result = activator.Activate(ItemPropertyPath);

            result.Should().NotBeNull();
            result.Should().BeOfType<NoConstructorSearch>();
        }

        private class Item
        {
            public string Property { get; set; }
        }

        private class NoConstructorSearch : AbstractSearch
        {
            public override bool IsActive() => throw new NotImplementedException();
            protected override Expression BuildFilterExpression(Expression property) => throw new NotImplementedException();
        }

        private class MultipleParameterConstructorSearch : AbstractSearch
        {
            public MultipleParameterConstructorSearch(string property, PropertyInfo propertyInfo)
            {
            }
            public override bool IsActive() => throw new NotImplementedException();
            protected override Expression BuildFilterExpression(Expression property) => throw new NotImplementedException();
        }

        private class MultipleConstructorsSearch : AbstractSearch
        {
            public MultipleConstructorsSearch(int noMatch)
            {
            }

            public MultipleConstructorsSearch(string property) : base(property)
            {
            }

            public MultipleConstructorsSearch(PropertyInfo propertyInfo) : base(propertyInfo.Name)
            {
            }

            public override bool IsActive() => throw new NotImplementedException();
            protected override Expression BuildFilterExpression(Expression property) => throw new NotImplementedException();
        }

        private class StringConstructorSearch : AbstractSearch
        {
            public StringConstructorSearch(string property) : base(property)
            {
            }

            public override bool IsActive() => throw new NotImplementedException();
            protected override Expression BuildFilterExpression(Expression property) => throw new NotImplementedException();
        }

        private class PrivateConstructorSearch : AbstractSearch
        {
            private PrivateConstructorSearch()
            {
            }

            public override bool IsActive() => throw new NotImplementedException();
            protected override Expression BuildFilterExpression(Expression property) => throw new NotImplementedException();
        }
    }
}