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
    public class IgnoredSearchTests : TestBase
    {
        private class IgnoredTestProfile : TestProfile
        {
            public IgnoredTestProfile()
            {
                IgnoreSearch(x => x.MatchAlways);
            }
        }


        private readonly Mock<ISearchConfigurationProvider> configurationProvider;

        public IgnoredSearchTests()
        {
            configurationProvider = new Mock<ISearchConfigurationProvider>();
            configurationProvider.Setup(x => x.ForEntityAndRequestType(It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns(SearchConfiguration);
        }

        protected override TestProfile CreateProfile()
        {
            return new IgnoredTestProfile();
        }

        [Fact]
        public void Ignored_Search_Is_Ignored()
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
            var result = entities.AsQueryable().Search(search).ToString();

            // Assert
            result.Should().NotContain("MatchAlways");

        }
    }
}