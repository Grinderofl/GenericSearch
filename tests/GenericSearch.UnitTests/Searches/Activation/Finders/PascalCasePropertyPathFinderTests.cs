using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation.Finders;
using Xunit;

namespace GenericSearch.UnitTests.Searches.Activation.Finders
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
