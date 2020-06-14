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
    public class IntegerSearchTests
    {
        [Fact]
        public void Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value));

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
        }

        [Fact]
        public void Equal_Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value))
            {
                Term1 = 3,
                Is = IntegerSearch.Comparer.Equal
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(3);
        }

        [Fact]
        public void Greater_Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value))
            {
                Term1 = 2,
                Is = IntegerSearch.Comparer.Greater
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(3);
        }

        [Fact]
        public void GreaterOrEqual_Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value))
            {
                Term1 = 2,
                Is = IntegerSearch.Comparer.GreaterOrEqual
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(2);
            result[1].Id.Should().Be(3);
        }

        [Fact]
        public void InRange_Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value))
            {
                Term1 = 1,
                Term2 = 2,
                Is = IntegerSearch.Comparer.InRange
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
        }

        [Fact]
        public void Less_Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value))
            {
                Term1 = 2,
                Is = IntegerSearch.Comparer.Less
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(1);
        }

        [Fact]
        public void LessOrEqual_Succeeds()
        {
            var search = new IntegerSearch(nameof(Item.Value))
            {
                Term1 = 3,
                Is = IntegerSearch.Comparer.LessOrEqual
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
            result[2].Id.Should().Be(3);
        }

        [Fact]
        public void Child_Succeeds()
        {
            Expression<Func<Foo, object>> expression = foo => foo.Bar.Value;
            var property = expression.ToString();
            var search = new IntegerSearch(property);

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

            var search = new IntegerSearch(property)
            {
                Is = IntegerSearch.Comparer.Equal,
                Term1 = 2
            };
            var result = search.ApplyToQuery(Foos().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(5);
        }

        [Fact]
        public void Children_Equals_Succeeds()
        {
            var search = new IntegerSearch($"Bars.{nameof(Bar.Value)}")
            {
                Is = IntegerSearch.Comparer.Equal,
                Term1 = 5
            };

            var result = search.ApplyToQuery(Foos2().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(6);
        }
        
        private class Item
        {
            public Item(int id, int value)
            {
                Id = id;
                Value = value;
            }
            
            public int Id { get; }
            public int Value { get; }
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
            public Bar(int id, int value)
            {
                Id = id;
                Value = value;
            }

            public int Id { get; }
            public int Value { get; }
        }

        private IEnumerable<Item> Items()
        {
            yield return new Item(1, 1);
            yield return new Item(2, 2);
            yield return new Item(3, 3);
        }

        private IEnumerable<Foo> Foos()
        {
            yield return new Foo(4, new Bar(1, 1));
            yield return new Foo(5, new Bar(2, 2));
        }

        private IEnumerable<Foo> Foos2()
        {
            yield return new Foo(6) {Bars = {new Bar(4, 5), new Bar(5, 10)}};
            yield return new Foo(7) {Bars = {new Bar(6, 15), new Bar(7, 20)}};
        }
    }
}