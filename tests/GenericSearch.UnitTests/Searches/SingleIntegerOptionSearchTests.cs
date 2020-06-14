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
    public class SingleSingleIntegerOptionOptionSearchTests
    {
        [Fact]
        public void Succeeds()
        {
            var search = new SingleIntegerOptionSearch(nameof(Item.Value));

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
        }

        [Fact]
        public void Is_Succeeds()
        {
            var search = new SingleIntegerOptionSearch(nameof(Item.Value))
            {
                Is = 3
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(3);
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