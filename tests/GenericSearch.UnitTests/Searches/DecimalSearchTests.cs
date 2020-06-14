using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Searches
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class DecimalSearchTests
    {
        [Fact]
        public void Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value));

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
        }

        [Fact]
        public void Equal_Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value))
            {
                Term1 = 3.5M,
                Is = DecimalSearch.Comparer.Equal
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(3);
        }

        [Fact]
        public void Greater_Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value))
            {
                Term1 = 2.1M,
                Is = DecimalSearch.Comparer.Greater
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(3);
        }

        [Fact]
        public void GreaterOrEqual_Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value))
            {
                Term1 = 2.1M,
                Is = DecimalSearch.Comparer.GreaterOrEqual
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(2);
            result[1].Id.Should().Be(3);
        }

        [Fact]
        public void InRange_Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value))
            {
                Term1 = 2.0M,
                Term2 = 2.2M,
                Is = DecimalSearch.Comparer.InRange
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(2);
        }

        [Fact]
        public void Less_Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value))
            {
                Term1 = 2.1M,
                Is = DecimalSearch.Comparer.Less
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(1);
        }

        [Fact]
        public void LessOrEqual_Succeeds()
        {
            var search = new DecimalSearch(nameof(Item.Value))
            {
                Term1 = 2.1M,
                Is = DecimalSearch.Comparer.LessOrEqual
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
        }

        [Fact]
        public void Child_Succeeds()
        {
            Expression<Func<Foo, object>> expression = foo => foo.Bar.Value;
            var property = expression.ToString();
            var search = new DecimalSearch(property);

            var result = search.ApplyToQuery(Foos().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(4);
            result[1].Id.Should().Be(5);
        }

        [Fact]
        public void Child_Equals_Succeeds()
        {
            Expression<Func<Foo, object>> expression = foo => foo.Bar.Value;
            var propert = expression.GetPropertyPath();
            var property = string.Join(".", PropertyPath<Foo>.Get(x => x.Bar.Value).Select(x => x.Name));

            var search = new DecimalSearch(property)
            {
                Is = DecimalSearch.Comparer.Equal,
                Term1 = 2.1M
            };
            var result = search.ApplyToQuery(Foos().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(5);
        }

        [Fact]
        public void Children_Equals_Succeeds()
        {
            var search = new DecimalSearch($"Bars.{nameof(Bar.Value)}")
            {
                Is = DecimalSearch.Comparer.Equal,
                Term1 = 5.0M
            };

            var result = search.ApplyToQuery(Foos2().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(6);
        }
        
        private class Item
        {
            public Item(int id, decimal value)
            {
                Id = id;
                Value = value;
            }
            
            public int Id { get; }
            public decimal Value { get; }
        }

        
        private class Foo
        {
            public Foo(int id)
            {
                Id = id;
            }

            public Foo(int id, Bar bar)
            {
                Id = id;
                Bar = bar;
            }

            public int Id { get; }
            public Bar Bar { get; }
            public ICollection<Bar> Bars { get; } = new List<Bar>();
        }

        private class Bar
        {
            public Bar(int id, decimal value)
            {
                Id = id;
                Value = value;
            }

            public int Id { get; }
            public decimal Value { get; }
        }

        private IEnumerable<Item> Items()
        {
            yield return new Item(1, 1.2M);
            yield return new Item(2, 2.1M);
            yield return new Item(3, 3.5M);
        }

        private IEnumerable<Foo> Foos()
        {
            yield return new Foo(4, new Bar(1, 1.2M));
            yield return new Foo(5, new Bar(2, 2.1M));
        }

        private IEnumerable<Foo> Foos2()
        {
            yield return new Foo(6) {Bars = {new Bar(4, 5.0M), new Bar(5, 10.1M)}};
            yield return new Foo(7) {Bars = {new Bar(6, 15.2M), new Bar(7, 20.3M)}};
        }
    }
}