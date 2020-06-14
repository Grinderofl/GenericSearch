using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Searches
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class MultipleTextOptionOptionSearchTests
    {
        [Fact]
        public void Succeeds()
        {
            var search = new MultipleTextOptionSearch(nameof(Item.Value));

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
        }

        [Fact]
        public void Is_Succeeds()
        {
            var search = new MultipleTextOptionSearch(nameof(Item.Value))
            {
                Is = new []{"Bar"}
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(2);
        }

        [Fact]
        public void Is_Multiple_Succeeds()
        {
            var search = new MultipleTextOptionSearch(nameof(Item.Value))
            {
                Is = new []{"Bar", "Baz"}
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(2);
            result[1].Id.Should().Be(3);
        }


        private class Item
        {
            public Item(int id, string value)
            {
                Id = id;
                Value = value;
            }
            
            public int Id { get; }
            public string Value { get; }
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
            public Bar(int id, string value)
            {
                Id = id;
                Value = value;
            }

            public int Id { get; }
            public string Value { get; }
        }

        private IEnumerable<Item> Items()
        {
            yield return new Item(1, "Foo");
            yield return new Item(2, "Bar");
            yield return new Item(3, "Baz");
        }

        private IEnumerable<Foo> Foos()
        {
            yield return new Foo(4, new Bar(1, "Foo"));
            yield return new Foo(5, new Bar(2, "Baz"));
        }

        private IEnumerable<Foo> Foos2()
        {
            yield return new Foo(6) {Bars = {new Bar(4, "Bar"), new Bar(5, "Baz")}};
            yield return new Foo(7) {Bars = {new Bar(6, "Foo"), new Bar(7, "Qux")}};
        }
    }
}