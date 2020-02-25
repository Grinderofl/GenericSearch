using FluentAssertions;
using Grinderofl.GenericSearch.Transformers;
using Xunit;

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public class RouteValueTransformerTests : TestBase
    {
        private readonly IRouteValueTransformer transformer;

        public RouteValueTransformerTests()
        {
            transformer = new RouteValueTransformer(ProcessorProvider.Object);
        }

        [Fact]
        public void Can_Transform_Default_Request()
        {
            // Arrange
            var request = new TestRequest();
            RequestBinder.BindRequest(request, SearchConfiguration);
            
            // Act
            var result = transformer.Transform(request);

            // Assert
            result.Count.Should().Be(0);
        }

        [Fact]
        public void Can_Transform_Populated_Request()
        {
            // Arrange
            var request = new TestRequest();
            RequestBinder.BindRequest(request, SearchConfiguration);
            request.MatchAlways.Is = true;
            request.MatchWhenTrue.Is = true;
            request.MatchWhenTrueDefaultTrue.Is = false;
            request.MatchWhenNotNull.Is = true;
            request.MatchWhenNotNullDefaultTrue.Is = false;
            request.MatchWhenNotNullDefaultFalse.Is = true;

            // Act
            var result = transformer.Transform(request);

            // Assert
            result.Count.Should().Be(6);
            result["matchalways.is"].Should().Be(true);
            result["matchwhentrue.is"].Should().Be(true);
            result["matchwhentruedefaulttrue.is"].Should().Be(false);
            result["matchwhennotnull.is"].Should().Be(true);
            result["matchwhennotnulldefaulttrue.is"].Should().Be(false);
            result["matchwhennotnulldefaultfalse.is"].Should().Be(true);
        }
    }
}