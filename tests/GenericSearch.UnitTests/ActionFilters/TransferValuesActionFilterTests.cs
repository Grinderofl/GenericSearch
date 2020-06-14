using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.ActionFilters;
using GenericSearch.Configuration;
using GenericSearch.Internal;
using GenericSearch.Searches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.ActionFilters
{
    public class TransferValuesActionFilterTests
    {
        [Fact]
        public async Task NoResult_Fails()
        {
            var modelProvider = new Mock<IModelProvider>();
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(0);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var actionExecutedContext = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object);
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task NoModel_Fails()
        {
            var modelProvider = new Mock<IModelProvider>();
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(0);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = new RedirectToActionResult("Index", "Controller", null)
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        private class TestRequest
        {
            public TextSearch Text { get; set; } = new TextSearch("Text");

        }

        private class TestItem
        {
            public string Text { get; set; }
        }

        private class TestResult
        {
            public TextSearch Text { get; set; }
        }
    }
}