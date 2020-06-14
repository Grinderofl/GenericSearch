using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.ActionFilters
{
    public class PostRedirectGetActionFilterTests
    {
        [Fact]
        public async Task NotPostRequest_Fails()
        {
            var modelProvider = new Mock<IModelProvider>();
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(1);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("GET");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task NotControllerActionDescriptor_Fails()
        {
            var modelProvider = new Mock<IModelProvider>();
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(1);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new PageActionDescriptor());

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("POST");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task NoModelToProvide_Fails()
        {
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(null);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);

            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());
            actionFilter.Order.Should().Be(1);
            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("POST");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task NoConfiguration_Fails()
        {
            var model = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(model);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns((ListConfiguration) null);

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(1);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("POST");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task ActionName_Mismatch_Fails()
        {
            var model = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(model);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var listConfiguration = new ListConfiguration(typeof(TestRequest), typeof(TestItem), typeof(TestResult))
            {
                PostRedirectGetConfiguration = new PostRedirectGetConfiguration("Index", true)
            };

            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration);

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(1);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor(){ActionName = "List"});

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("POST");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task Enabled_False_Fails()
        {
            var model = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(model);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var listConfiguration = new ListConfiguration(typeof(TestRequest), typeof(TestItem), typeof(TestResult))
            {
                PostRedirectGetConfiguration = new PostRedirectGetConfiguration("Index", false)
            };

            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration);

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(1);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor(){ActionName = "Index"});

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("POST");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task Succeeds()
        {
            var model = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(model);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var listConfiguration = new ListConfiguration(typeof(TestRequest), typeof(TestItem), typeof(TestResult))
            {
                PostRedirectGetConfiguration = new PostRedirectGetConfiguration("Index", true),
                PageConfiguration = new PageConfiguration("Page", 1),
                RowsConfiguration = new RowsConfiguration("Rows", 20),
                SortDirectionConfiguration = new SortDirectionConfiguration("Ordx", Direction.Ascending)
            };

            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration);

            var actionFilter = new PostRedirectGetActionFilter(modelProvider.Object, configurationProvider.Object);
            actionFilter.Order.Should().Be(1);
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor(){ActionName = "Index"});

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();

            httpRequest.SetupGet(x => x.Method).Returns("POST");
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);

            var controller = new Mock<Controller>();

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeOfType<RedirectToActionResult>();
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
