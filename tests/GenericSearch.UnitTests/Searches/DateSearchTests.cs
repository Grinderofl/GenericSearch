using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Searches
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class DateSearchTests
    {
        [Fact]
        public void Succeeds()
        {
            var search = new DateSearch("Value");

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(6);
        }

        [Fact]
        public void Less_Succeeds()
        {
            var search = new DateSearch("Value")
            {
                Is = DateSearch.Comparer.Less,
                Term1 = new DateTime(2020, 1, 3)
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(2);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
        }

        [Fact]
        public void LessOrEqual_Succeeds()
        {
            var search = new DateSearch("Value")
            {
                Is = DateSearch.Comparer.LessOrEqual,
                Term1 = new DateTime(2020, 1, 3)
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
            result[0].Id.Should().Be(1);
            result[1].Id.Should().Be(2);
            result[2].Id.Should().Be(3);
        }

        [Fact]
        public void Equal_Succeeds()
        {
            var search = new DateSearch("Value")
            {
                Is = DateSearch.Comparer.Equal,
                Term1 = new DateTime(2020, 1, 1)
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(1);
            result[0].Id.Should().Be(1);
        }


        [Fact]
        public void GreaterOrEqual_Succeeds()
        {
            var search = new DateSearch("Value")
            {
                Is = DateSearch.Comparer.GreaterOrEqual,
                Term1 = new DateTime(2020, 1, 3)
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(4);
            result[0].Id.Should().Be(3);
            result[1].Id.Should().Be(4);
            result[2].Id.Should().Be(5);
            result[3].Id.Should().Be(6);
        }

        [Fact]
        public void Greater_Succeeds()
        {
            var search = new DateSearch("Value")
            {
                Is = DateSearch.Comparer.Greater,
                Term1 = new DateTime(2020, 1, 3)
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
            result[0].Id.Should().Be(4);
            result[1].Id.Should().Be(5);
            result[2].Id.Should().Be(6);
        }

        [Fact]
        public void InRange_Succeeds()
        {
            var search = new DateSearch("Value")
            {
                Is = DateSearch.Comparer.InRange,
                Term1 = new DateTime(2020, 1, 2),
                Term2 = new DateTime(2020, 1, 4)
            };

            var result = search.ApplyToQuery(Items().AsQueryable()).ToArray();

            result.Length.Should().Be(3);
            result[0].Id.Should().Be(2);
            result[1].Id.Should().Be(3);
            result[2].Id.Should().Be(4);
        }



        private class Item
        {
            public Item(int id, DateTime value)
            {
                Id = id;
                Value = value;
            }

            public int Id { get; }
            public DateTime Value { get; }
        }
        
        private IEnumerable<Item> Items()
        {
            yield return new Item(1, new DateTime(2020, 1, 1));
            yield return new Item(2, new DateTime(2020, 1, 2));
            yield return new Item(3, new DateTime(2020, 1, 3));
            yield return new Item(4, new DateTime(2020, 1, 4));
            yield return new Item(5, new DateTime(2020, 1, 5));
            yield return new Item(6, new DateTime(2020, 1, 6));
        }
    }
}