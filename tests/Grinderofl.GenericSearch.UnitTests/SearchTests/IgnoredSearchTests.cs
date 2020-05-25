using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Grinderofl.GenericSearch.Extensions;
using Grinderofl.GenericSearch.ModelBinding;
using Grinderofl.GenericSearch.UnitTests.BooleanSearchTests;
using Xunit;

namespace Grinderofl.GenericSearch.UnitTests.SearchTests
{
    public class IgnoredSearchTests : TestBase
    {
        private class IgnoredTestProfile : TestProfile
        {
            protected override IFilterExpression<TestEntity, TestRequest, TestResult> CreateFilterCore()
            {
                return base.CreateFilterCore().Boolean(x => x.MatchAlways, x => x.Ignore());
            }
        }
        
        protected override TestProfile CreateProfile()
        {
            return new IgnoredTestProfile();
        }

        [Fact]
        public async Task Ignored_Search_Is_Ignored()
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
            var result = entities.AsQueryable().Search(search).ToString();

            // Assert
            result.Should().NotContain("MatchAlways");

        }
    }
}