using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.ActionFilters;
using GenericSearch.Configuration;
using GenericSearch.Internal;
using GenericSearch.Searches;
using GenericSearch.UnitTests.ModelBinders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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
            modelProvider.Verify(x => x.Provide(), Times.Never);
        }

        [Fact]
        public async Task NoResultModel_Fails()
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
            modelProvider.Verify(x => x.Provide(), Times.Never);
        }

        [Fact]
        public async Task NoRequestModel_Fails()
        {
            var modelProvider = new Mock<IModelProvider>();
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var result = new JsonResult(new TestResult());
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = result
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
            configurationProvider.Verify(x => x.GetConfiguration(It.IsAny<Type>()), Times.Never);
        }

        [Fact]
        public async Task NoConfiguration_Fails()
        {
            var requestModel = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(requestModel);
            var configurationProvider = new Mock<IListConfigurationProvider>();

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var result = new JsonResult(new TestResult());
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = result
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task ConfigurationResultType_ResultModelType_Mismatch_Fails()
        {
            var requestModel = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(requestModel);

            var listConfiguration = new Mock<IListConfiguration>();
            listConfiguration.SetupGet(x => x.ResultType).Returns(typeof(TestItem));

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration.Object);

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var result = new JsonResult(new TestResult());
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = result
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task RouteValues_Action_Mismatch_Fails()
        {
            var requestModel = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(requestModel);

            var transferValuesConfiguration = new Mock<ITransferValuesConfiguration>();
            transferValuesConfiguration.SetupGet(x => x.ActionName).Returns("Index");

            var listConfiguration = new Mock<IListConfiguration>();
            listConfiguration.SetupGet(x => x.ResultType).Returns(typeof(TestResult));
            listConfiguration.SetupGet(x => x.TransferValuesConfiguration).Returns(transferValuesConfiguration.Object);

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration.Object);

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(),
                                                  new ControllerActionDescriptor()
                                                  {
                                                      ActionName = "List",
                                                      RouteValues = new Dictionary<string, string>() {["action"] = "List"}
                                                  });
            
            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var result = new JsonResult(new TestResult());
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = result
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact]
        public async Task Disabled_Fails()
        {
            var requestModel = new TestRequest();
            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(requestModel);

            var transferValuesConfiguration = new Mock<ITransferValuesConfiguration>();
            transferValuesConfiguration.SetupGet(x => x.ActionName).Returns("Index");
            transferValuesConfiguration.SetupGet(x => x.Enabled).Returns(false);

            var listConfiguration = new Mock<IListConfiguration>();
            listConfiguration.SetupGet(x => x.ResultType).Returns(typeof(TestResult));
            listConfiguration.SetupGet(x => x.TransferValuesConfiguration).Returns(transferValuesConfiguration.Object);

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration.Object);

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(),
                                                  new ControllerActionDescriptor()
                                                  {
                                                      ActionName = "Index",
                                                      RouteValues = new Dictionary<string, string>() {["action"] = "Index"}
                                                  });
            
            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var result = new JsonResult(new TestResult());
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = result
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            actionExecutingContext.Result.Should().BeNull();
        }

        private static Type RequestType = typeof(TestRequest);
        private static Type ItemType = typeof(TestItem);
        private static Type ResultType = typeof(TestResult);

        [Fact]
        public async Task Succeeds()
        {
            var requestModel = new TestRequest();
            var resultModel = new TestResult();

            var modelProvider = new Mock<IModelProvider>();
            modelProvider.Setup(x => x.Provide()).Returns(requestModel);

            var transferValuesConfiguration = new Mock<ITransferValuesConfiguration>();
            transferValuesConfiguration.SetupGet(x => x.ActionName).Returns("Index");
            transferValuesConfiguration.SetupGet(x => x.Enabled).Returns(true);

            var searchConfiguration = new SearchConfiguration(typeof(TestRequest).GetProperty(nameof(TestRequest.Text)))
            {
                ResultProperty = typeof(TestResult).GetProperty(nameof(TestResult.Text))
            };
            var searchConfigurations = new List<ISearchConfiguration> {searchConfiguration};

            var propertyConfiguration = new PropertyConfiguration(RequestType.GetProperty("Foo"), ResultType.GetProperty("Foo"), "Bar", false);
            var propertyConfigurations = new List<IPropertyConfiguration>(){propertyConfiguration};

            var pageConfiguration = new PageConfiguration(RequestType.GetProperty("Page"), ResultType.GetProperty("Page"), 1);
            var rowsConfiguration = new RowsConfiguration(RequestType.GetProperty("Rows"), ResultType.GetProperty("Rows"), 200);
            var sortColumnConfiguration = new SortColumnConfiguration(RequestType.GetProperty("Ordx"), ResultType.GetProperty("Ordx"), "");
            var sortDirectionConfiguration = new SortDirectionConfiguration(RequestType.GetProperty("Ordd"), ResultType.GetProperty("Ordd"), Direction.Ascending);

            var listConfiguration = new Mock<IListConfiguration>();
            listConfiguration.SetupGet(x => x.ResultType).Returns(typeof(TestResult));
            listConfiguration.SetupGet(x => x.TransferValuesConfiguration).Returns(transferValuesConfiguration.Object);
            listConfiguration.SetupGet(x => x.SearchConfigurations).Returns(searchConfigurations);
            listConfiguration.SetupGet(x => x.PropertyConfigurations).Returns(propertyConfigurations);
            listConfiguration.SetupGet(x => x.PageConfiguration).Returns(pageConfiguration);
            listConfiguration.SetupGet(x => x.RowsConfiguration).Returns(rowsConfiguration);
            listConfiguration.SetupGet(x => x.SortColumnConfiguration).Returns(sortColumnConfiguration);
            listConfiguration.SetupGet(x => x.SortDirectionConfiguration).Returns(sortDirectionConfiguration);

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(listConfiguration.Object);

            var actionFilter = new TransferValuesActionFilter(modelProvider.Object, configurationProvider.Object);
            
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, new RouteData(),
                                                  new ControllerActionDescriptor()
                                                  {
                                                      ActionName = "Index",
                                                      RouteValues = new Dictionary<string, string>() {["action"] = "Index"}
                                                  });
            
            var controller = new Mock<Controller>();

            var httpRequest = new Mock<HttpRequest>();
            var actionExecutionDelegate = new Mock<ActionExecutionDelegate>();
            var result = new JsonResult(resultModel);
            var actionExecutedContext =
                new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller.Object)
                {
                    Result = result
                };
            actionExecutionDelegate.Setup(x => x.Invoke()).ReturnsAsync(() => actionExecutedContext);
            
            httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
            
            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new RouteValueDictionary(), controller.Object);
            
            await actionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate.Object);

            resultModel.Text.Should().BeOfType<TextSearch>();
        }


        private class TestRequest
        {
            public TextSearch Text { get; set; } = new TextSearch("Text");
            public string Foo { get; set; } = "Bar";
            public int Page { get; set; } = 1;
            public int Rows { get; set; } = 200;
            public string Ordx { get; set; } = "Text";
            public Direction Ordd { get; set; } = Direction.Descending;
        }

        private class TestItem
        {
            public string Text { get; set; }
        }

        private class TestResult
        {
            public TextSearch Text { get; set; }
            public string Foo { get; set; }
            public int Page { get; set; }
            public int Rows { get; set; }
            public string Ordx { get; set; }
            public Direction Ordd { get; set; }
        }
    }
}