using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Searches
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class OptionalBooleanSearchTests
    {
    [Fact]
    public void Succeeds()
    {
        var search = new OptionalBooleanSearch("Value");

        var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

        result.Length.Should().Be(2);
    }

    [Fact]
    public void True_Succeeds()
    {
        var search = new OptionalBooleanSearch("Value")
        {
            Is = true
        };

        var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

        result.Length.Should().Be(1);
        result[0].Id.Should().Be(1);
    }

    [Fact]
    public void False_Succeeds()
    {
        var search = new OptionalBooleanSearch("Value")
        {
            Is = false
        };

        var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

        result.Length.Should().Be(1);
        result[0].Id.Should().Be(2);
    }



    private class Item
    {
        public Item(int id, bool value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; }
        public bool Value { get; }
    }

    private IEnumerable<Item> Items()
    {
        yield return new Item(1, true);
        yield return new Item(2, false);
    }
    }
}