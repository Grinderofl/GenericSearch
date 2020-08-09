using FluentAssertions;
using GenericSearch.Internal.Activation.Finders;
using GenericSearch.Searches;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Activation.Finders
{
    public class PascalCasePropertyPathFinderTests
    {
        private readonly PascalCasePropertyPathFinder propertyPathFinder = new PascalCasePropertyPathFinder();

        [Fact]
        public void No_Match_Succeeds()
        {
            propertyPathFinder.Find(typeof(Item), "ItemChildBar").Should().BeNull();
        }

        private class Request
        {
            public TextSearch ItemChildFoo { get; set; }
            public TextSearch ItemChildBar { get; set; }
        }

        private class Item
        {
            public Child ItemChild { get; set; }
        }

        private class Child
        {
            public string Foo { get; set; }
        }

        private class Result
        {
            public TextSearch ItemChildFoo { get; set; }
            public TextSearch ItemChildBar { get; set; }
        }
    }
}
