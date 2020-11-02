using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FluentAssertions;
using GenericSearch.ActionFilters;
using GenericSearch.Searches;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace GenericSearch.UnitTests.ActionFilters
{
    public class RouteValueTransformerTests
    {

        [Fact]
        public void Can_Transform()
        {
            var transformer = new GenericSearchRouteValueTransformer();
            var request = new TestRequest()
            {
                Bar = "Baz",
                Baz = new TextSearch(){Term = "Bro"},
                Bro = new []{"Hello", "World"}
            };

            var rvd = transformer.Transform(request, typeof(TestRequest));
            rvd.Should().NotBeNull();
        }

        private class TestRequest
        {
            [DefaultValue(null)]
            public string Foo { get; set; }

            [DefaultValue("Baz")]
            public string Bar { get; set; }

            public TextSearch Baz { get; set; }

            public string[] Bro { get; set; }

            [BindNever]
            public string Skippy { get; set; }

            [BindNever]
            public TextSearch Skippity { get; set; }

            public Direction Direction { get; set; } = Direction.Ascending;
        }
    }
}
