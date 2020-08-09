using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.ActionFilters;
using GenericSearch.Internal;
using GenericSearch.Internal.Configuration;
using GenericSearch.Searches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var modelProvider = new Mock<IRequestModelProvider>();
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
            var modelProvider = new Mock<IRequestModelProvider>();
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
            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(null);
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
            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);
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
            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);
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
            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);
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
            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var listConfiguration = new ListConfiguration(typeof(TestRequest), typeof(TestItem), typeof(TestResult))
            {
                PostRedirectGetConfiguration = new PostRedirectGetConfiguration("Index", true),
                PageConfiguration = new PageConfiguration("Page", 1),
                RowsConfiguration = new RowsConfiguration("Rows", 20),
                SortColumnConfiguration = new SortColumnConfiguration("Ordx", "Text"),
                SortDirectionConfiguration = new SortDirectionConfiguration("Ordd", Direction.Ascending)
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

            public IntegerSearch Integer { get; set; } = new IntegerSearch("Integer") {Term1 = 1};

            [BindNever]
            public TextSearch Foo { get; set; }

            public TestSearch FooIgnored { get; set; } = new TestSearch();

            [DefaultValue(1)]
            public int Bar { get; set; } = 1;

            [DefaultValue(1)]
            public int Page { get; set; } = 1;

            [DefaultValue(20)]
            public int Rows { get; set; } = 20;

            [DefaultValue("Text")]
            public string Ordx { get; set; } = "Text";

            [DefaultValue(Direction.Ascending)]
            public Direction Ordd { get; set; } = Direction.Ascending;

            public string Extra { get; set; }

            public string[] Extras { get; set; } = {"Foo", "Bar"};
            public TestEnum TestEnum { get; set; }

            [DefaultValue(null)]
            public string Baz { get; set; } = "FooBar";
        }

        private enum TestEnum
        {
            Foo,
            Bar
        }

        private class TestItem
        {
            public string Text { get; set; }
        }

        private class TestResult
        {
            public TextSearch Text { get; set; }
            public IntegerSearch Integer { get; set; }
            public TestSearch FooIgnored { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
            public TextSearch Foo { get; set; }
        }

        private class TestSearch : ISearch
        {
            [BindNever]
            public string IgnoreMe { get; set; }
            public bool IsActive() => false;

            public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
            {
                throw new NotImplementedException();
            }
        }
    }
}
