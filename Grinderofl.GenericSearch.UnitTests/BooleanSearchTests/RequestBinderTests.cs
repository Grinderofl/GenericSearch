using FluentAssertions;
using Xunit;

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public class RequestBinderTests : TestBase
    {
        [Fact]
        public void RequestBinder_Binds_Default_Request()
        {
            // Arrange
            var request = new TestRequest();

            // Act
            RequestBinder.BindRequest(request, SearchConfiguration);

            // Assert
            request.MatchAlways.Should().NotBeNull();
            request.MatchAlways.Is.Should().BeFalse();

            request.MatchWhenTrue.Should().NotBeNull();
            request.MatchWhenTrue.Is.Should().BeFalse();

            request.MatchWhenTrueDefaultTrue.Should().NotBeNull();
            request.MatchWhenTrueDefaultTrue.Is.Should().BeTrue();

            request.MatchWhenNotNull.Should().NotBeNull();
            request.MatchWhenNotNull.Is.Should().BeNull();

            request.MatchWhenNotNullDefaultTrue.Should().NotBeNull();
            request.MatchWhenNotNullDefaultTrue.Is.Should().BeTrue();

            request.MatchWhenNotNullDefaultFalse.Should().NotBeNull();
            request.MatchWhenNotNullDefaultFalse.Is.Should().BeFalse();
        }
    }
}