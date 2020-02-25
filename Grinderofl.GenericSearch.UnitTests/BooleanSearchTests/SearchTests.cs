using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Extensions;
using Moq;
using Xunit;

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public class SearchTests : TestBase
    {
        private readonly Mock<ISearchConfigurationProvider> configurationProvider;

        public SearchTests()
        {
            configurationProvider = new Mock<ISearchConfigurationProvider>();
            configurationProvider.Setup(x => x.ForEntityAndRequestType(It.IsAny<Type>(), It.IsAny<Type>()))
                                 .Returns(SearchConfiguration);
        }

        [Fact]
        public void Can_Search_Default_Request()
        {
            // Arrange
            var entities = new List<TestEntity>(TestEntityHelper.CreateEntities());
            var request = new TestRequest();
            RequestBinder.BindRequest(request, SearchConfiguration);
            var cacheProvider = new TestRequestModelCacheProvider(request);
            var search = new GenericSearch(configurationProvider.Object, cacheProvider);

            // Act
            var result = entities.AsQueryable().Search(search).ToList();

            // Assert
            result.Count.Should().Be(0);
        }

        [Fact]
        public void Can_Search_WhenAllTrue_Request()
        {
            // Arrange
            var entities = new List<TestEntity>(TestEntityHelper.CreateEntities());
            var request = new TestRequest();
            RequestBinder.BindRequest(request, SearchConfiguration);
            request.MatchAlways.Is = true;
            request.MatchWhenNotNull.Is = true;
            request.MatchWhenNotNullDefaultFalse.Is = true;
            var cacheProvider = new TestRequestModelCacheProvider(request);
            var search = new GenericSearch(configurationProvider.Object, cacheProvider);

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