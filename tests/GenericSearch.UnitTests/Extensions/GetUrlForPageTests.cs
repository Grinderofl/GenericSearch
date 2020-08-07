using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Internal;
using GenericSearch.Internal.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Extensions
{
    public class GetUrlForPageTests
    {
        [Fact]
        public void ModelProvider_NotRegistered_Throws()
        {
            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(null);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            Assert.Throws<InvalidOperationException>(() =>
            {
                htmlHelper.Object.GetUrlForPage(2);
            });
        }

        [Fact]
        public void Model_Null_Throws()
        {
            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(null);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            Assert.Throws<ModelProviderException>(() =>
            {
                htmlHelper.Object.GetUrlForPage(2);
            });
        }

        [Fact]
        public void ListConfigurationProvider_Null_Throws()
        {
            var model = new Model();

            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns((IListConfiguration) null);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            Assert.Throws<InvalidOperationException>(() =>
            {
                htmlHelper.Object.GetUrlForPage(2);
            });
        }

        [Fact]
        public void ListConfiguration_Null_Throws()
        {
            var model = new Model();

            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);

            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns((IListConfiguration) null);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);
            requestServices.Setup(x => x.GetService(typeof(IListConfigurationProvider))).Returns(configurationProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            Assert.Throws<MissingConfigurationException>(() =>
            {
                htmlHelper.Object.GetUrlForPage(2);
            });
        }

        [Fact]
        public void PageConfiguration_Null_Throws()
        {
            var model = new Model();

            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);

            var configuration = new Mock<IListConfiguration>();
            configuration.SetupGet(x => x.PageConfiguration).Returns((IPageConfiguration)null);
            
            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(configuration.Object);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);
            requestServices.Setup(x => x.GetService(typeof(IListConfigurationProvider))).Returns(configurationProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            Assert.Throws<NullReferenceException>(() =>
            {
                htmlHelper.Object.GetUrlForPage(2);
            });
        }

        [Fact]
        public void DefaultPage_Removes_Parameter()
        {
            const string queryString = "?page=1";

            var model = new Model();

            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);

            var pageConfiguration = new Mock<IPageConfiguration>();
            pageConfiguration.SetupGet(x => x.DefaultValue).Returns(1);
            pageConfiguration.SetupGet(x => x.Name).Returns("page");

            var configuration = new Mock<IListConfiguration>();
            configuration.SetupGet(x => x.PageConfiguration).Returns(pageConfiguration.Object);
            
            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(configuration.Object);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);
            requestServices.Setup(x => x.GetService(typeof(IListConfigurationProvider))).Returns(configurationProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object,
                Request =
                {
                    QueryString = new QueryString(queryString)
                }
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            
            var actual = htmlHelper.Object.GetUrlForPage(1);
            actual.Should().Be("");
        }

        [Fact]
        public void Page_Adds_Parameter()
        {
            const string queryString = "";

            var model = new Model();

            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);

            var pageConfiguration = new Mock<IPageConfiguration>();
            pageConfiguration.SetupGet(x => x.DefaultValue).Returns(1);
            pageConfiguration.SetupGet(x => x.Name).Returns("page");

            var configuration = new Mock<IListConfiguration>();
            configuration.SetupGet(x => x.PageConfiguration).Returns(pageConfiguration.Object);
            
            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(configuration.Object);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);
            requestServices.Setup(x => x.GetService(typeof(IListConfigurationProvider))).Returns(configurationProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object,
                Request =
                {
                    QueryString = new QueryString(queryString)
                }
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            
            var actual = htmlHelper.Object.GetUrlForPage(2);
            actual.Should().Be("?page=2");
        }

        [Fact]
        public void Page_Updates_Parameter()
        {
            const string queryString = "?page=3";

            var model = new Model();

            var modelProvider = new Mock<IRequestModelProvider>();
            modelProvider.Setup(x => x.GetCurrentRequestModel()).Returns(model);

            var pageConfiguration = new Mock<IPageConfiguration>();
            pageConfiguration.SetupGet(x => x.DefaultValue).Returns(1);
            pageConfiguration.SetupGet(x => x.Name).Returns("page");

            var configuration = new Mock<IListConfiguration>();
            configuration.SetupGet(x => x.PageConfiguration).Returns(pageConfiguration.Object);
            
            var configurationProvider = new Mock<IListConfigurationProvider>();
            configurationProvider.Setup(x => x.GetConfiguration(It.IsAny<Type>())).Returns(configuration.Object);

            var requestServices = new Mock<IServiceProvider>();
            requestServices.Setup(x => x.GetService(typeof(IRequestModelProvider))).Returns(modelProvider.Object);
            requestServices.Setup(x => x.GetService(typeof(IListConfigurationProvider))).Returns(configurationProvider.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = requestServices.Object,
                Request =
                {
                    QueryString = new QueryString(queryString)
                }
            };

            var viewContext = new ViewContext()
            {
                HttpContext = httpContext
            };
            
            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper.SetupGet(x => x.ViewContext).Returns(viewContext);

            
            var actual = htmlHelper.Object.GetUrlForPage(2);
            actual.Should().Be("?page=2");
        }

        private class Model
        {
            public int Page { get; set; }
        }
    }
}
