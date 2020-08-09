using System;
using System.Threading.Tasks;
using FluentAssertions;
using GenericSearch.Internal;
using GenericSearch.Internal.Activation;
using GenericSearch.Internal.Configuration;
using GenericSearch.ModelBinding;
using GenericSearch.Searches;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.ModelBinders
{
    public class GenericSearchModelBinderTests
    {
        public GenericSearchModelBinderTests()
        {
            
        }

        private ModelBindingContext CreateContext()
        {
            var bindingContext = new Mock<ModelBindingContext>();
            bindingContext.SetupGet(x => x.ModelType).Returns(typeof(Request));
            bindingContext.SetupProperty(x => x.Model);
            return bindingContext.Object;
        }

        private readonly ListConfiguration configuration = new ListConfiguration(typeof(Request), typeof(Item), typeof(Result));

        [Fact]
        public async Task  Activation_Succeeds()
        {
            var request = new Request();

            var requestActivator = new Mock<IModelActivator>();
            requestActivator.Setup(x => x.Activate(It.IsAny<ListConfiguration>())).Returns(() => request);

            var requestPropertyActivator = new Mock<IModelPropertyActivator>();
            requestPropertyActivator.Setup(x => x.Activate(It.IsAny<ListConfiguration>(), It.IsAny<object>()));

            var fallbackModelBinder = new Mock<IModelBinder>();
            var modelCache = new StaticModelCache();
            var modelBinder = new GenericSearchModelBinder(configuration, requestActivator.Object, requestPropertyActivator.Object, modelCache, fallbackModelBinder.Object);

            var bindingContext = CreateContext();

            await modelBinder.BindModelAsync(bindingContext);

            bindingContext.Model.Should().BeOfType<Request>();
            fallbackModelBinder.Verify(x => x.BindModelAsync(It.IsAny<ModelBindingContext>()), Times.Once);
            requestPropertyActivator.Verify(x => x.Activate(It.IsAny<ListConfiguration>(), It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task Null_Activation_Succeeds()
        {
            var requestActivator = new Mock<IModelActivator>();
            requestActivator.Setup(x => x.Activate(It.IsAny<ListConfiguration>())).Returns(() => null);

            var requestPropertyActivator = new Mock<IModelPropertyActivator>();
            requestPropertyActivator.Setup(x => x.Activate(It.IsAny<ListConfiguration>(), It.IsAny<object>()));

            var fallbackModelBinder = new Mock<IModelBinder>();
            var modelCache = new StaticModelCache();
            var modelBinder = new GenericSearchModelBinder(configuration, requestActivator.Object, requestPropertyActivator.Object, modelCache, fallbackModelBinder.Object);

            var bindingContext = CreateContext();

            await modelBinder.BindModelAsync(bindingContext);

            fallbackModelBinder.Verify(x => x.BindModelAsync(It.IsAny<ModelBindingContext>()), Times.Once);
            requestPropertyActivator.Verify(x => x.Activate(It.IsAny<ListConfiguration>(), It.IsAny<object>()), Times.Never);
        }

        private class Request
        {
            public DateSearch Date { get; set; }
            public DecimalSearch Decimal { get; set; }
            public IntegerSearch Integer { get; set; }
            public MultipleDateOptionSearch MultipleDateOption { get; set; }
            public MultipleIntegerOptionSearch MultipleIntegerOption { get; set; }
            public MultipleTextOptionSearch MultipleTextOption { get; set; }
            public OptionalBooleanSearch OptionalBoolean { get; set; }
            public SingleDateOptionSearch SingleDateOption { get; set; }
            public SingleIntegerOptionSearch SingleIntegerOption { get; set; }
            public SingleTextOptionSearch SingleTextOption { get; set; }
            public TextSearch Text { get; set; }
        }

        private class Item
        {
            public DateTime Date { get; set; }
            public decimal Decimal { set; get; }
            public int Integer { get; set; }
            public DateTime MultipleDateOption { get; set; }
            public int MultipleIntegerOption { get; set; }
            public string MultipleTextOption { get; set; }
            public bool OptionalBoolean { get; set; }
            public DateTime SingleDateOption { get; set; }
            public int SingleIntegerOption { get; set; }
            public string SingleTextOption { get; set; }
            public string Text { get; set; }
        }

        private class Result
        {
            public DateSearch Date { get; set; }
            public DecimalSearch Decimal { get; set; }
            public IntegerSearch Integer { get; set; }
            public MultipleDateOptionSearch MultipleDateOption { get; set; }
            public MultipleIntegerOptionSearch MultipleIntegerOption { get; set; }
            public MultipleTextOptionSearch MultipleTextOption { get; set; }
            public OptionalBooleanSearch OptionalBoolean { get; set; }
            public SingleDateOptionSearch SingleDateOption { get; set; }
            public SingleIntegerOptionSearch SingleIntegerOption { get; set; }
            public SingleTextOptionSearch SingleTextOption { get; set; }
            public TextSearch Text { get; set; }
        }
    }
}