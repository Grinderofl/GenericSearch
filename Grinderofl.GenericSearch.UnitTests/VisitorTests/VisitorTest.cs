using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Grinderofl.GenericSearch.ActionFilters;
using Grinderofl.GenericSearch.ActionFilters.Visitors;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Configuration.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Xunit;

namespace Grinderofl.GenericSearch.UnitTests.VisitorTests
{
    public class VisitorTest
    {

        [Fact]
        public void Should_Visit()
        {
            var model = new TestRequest();
            var rvd = new RouteValueDictionary();
            var configuration = new FilterConfiguration(typeof(TestEntity), typeof(TestRequest), typeof(TestResult))
            {
                PageConfiguration = new PageConfiguration(),
                SortConfiguration = new SortConfiguration(),
                CopyRequestFilterValuesConfiguration = new CopyRequestFilterValuesConfiguration(),
                RedirectPostToGetConfiguration = new RedirectPostToGetConfiguration(),
                ListActionName = "Index",
                SearchConfigurations = new List<ISearchConfiguration>()
            };
            var visitor = new ModelPropertyVisitor(configuration, model, rvd);

            foreach (var property in model.GetType().GetProperties())
            {
                visitor.Visit(property);
            }

            var url = $"?" + rvd.Select(s => $"{s.Key}={s.Value}")
                                .Aggregate((current, next) => $"{current},{next}");

            url.Should().Be("?five.is=3,page=3");
        }

    }
}
