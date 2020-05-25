using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.ModelBinding;
using Grinderofl.GenericSearch.UnitTests.BooleanSearchTests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Grinderofl.GenericSearch.UnitTests.SearchTests
{
    public class SearchTests : TestBase
    {



        [Fact]
        public async Task Can_Search_Default_Request()
        {
            // Arrange
            var entities = new List<TestEntity>(TestEntityHelper.CreateEntities());
            var configuration = Provider.Provide(typeof(TestRequest));
            var cacheProvider = new TestRequestModelCacheProvider();
            var binder = new GenericSearchModelBinder(configuration, new NullModelBinder(), cacheProvider);
            var context = new TestModelBindingContext();
            await binder.BindModelAsync(context);
            
            var search = new GenericSearch(Provider, cacheProvider);

            // Act
            var result = entities.AsQueryable().Search(search).ToList();

            // Assert
            result.Count.Should().Be(1);
        }

        [Fact]
        public async Task Can_Search_WhenAllTrue_Request()
        {
            // Arrange
            var entities = new List<TestEntity>(TestEntityHelper.CreateEntities());
            var configuration = Provider.Provide(typeof(TestRequest));
            var cacheProvider = new TestRequestModelCacheProvider();
            var binder = new GenericSearchModelBinder(configuration, new NullModelBinder(), cacheProvider);
            var context = new TestModelBindingContext();
            await binder.BindModelAsync(context);
            ((TestRequest) context.Model).MatchAlways.Is = true;
            ((TestRequest) context.Model).MatchWhenNotNull.Is = true;
            ((TestRequest) context.Model).MatchWhenNotNullDefaultFalse.Is = true;

            var search = new GenericSearch(Provider, cacheProvider);

            // Act
            var result = entities.AsQueryable().Search(search).ToList();
            
            // Assert
            result.Count.Should().Be(2);
            result[0].MatchAlways.Should().BeTrue();
            result[0].MatchWhenNotNull.Should().BeTrue();
            result[0].MatchWhenNotNullDefaultFalse.Should().BeTrue();
            result[0].MatchWhenNotNullDefaultTrue.Should().BeTrue();
            result[0].MatchWhenTrue.Should().BeTrue();
            result[0].MatchWhenTrueDefaultTrue.Should().BeTrue();
            
            result[1].MatchAlways.Should().BeTrue();
            result[1].MatchWhenNotNull.Should().BeTrue();
            result[1].MatchWhenNotNullDefaultFalse.Should().BeTrue();
            result[1].MatchWhenNotNullDefaultTrue.Should().BeTrue();
            result[1].MatchWhenTrue.Should().BeFalse();
            result[1].MatchWhenTrueDefaultTrue.Should().BeTrue();
        }
    }
}