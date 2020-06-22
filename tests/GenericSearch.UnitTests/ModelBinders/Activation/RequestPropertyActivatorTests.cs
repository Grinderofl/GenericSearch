﻿using System;
using FluentAssertions;
using GenericSearch.Configuration.Factories;
using GenericSearch.ModelBinders.Activation;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation;
using GenericSearch.Searches.Activation.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.ModelBinders.Activation
{
    public class RequestPropertyActivatorTests
    {
        private readonly Request model;

        public RequestPropertyActivatorTests()
        {
            var definition = TestListDefinition.Create<Request, Item, Result>();
            var mock = new Mock<IOptions<GenericSearchOptions>>();
            mock.Setup(x => x.Value)
                .Returns(new GenericSearchOptions());
            var options = mock.Object;

            var factory = new ListConfigurationFactory(new SearchConfigurationFactory(new PascalCasePropertyPathFinder()),
                                                       new PageConfigurationFactory(options),
                                                       new RowsConfigurationFactory(options),
                                                       new SortColumnConfigurationFactory(options),
                                                       new SortDirectionConfigurationFactory(options),
                                                       new PropertyConfigurationFactory(),
                                                       new PostRedirectGetConfigurationFactory(options),
                                                       new TransferValuesConfigurationFactory(options),
                                                       new RequestFactoryConfigurationFactory(options));

            var configuration = factory.Create(definition);
            
            var serviceProvider = new ServiceCollection().BuildServiceProvider();

            model = new Request();
            
            var activatorFactory = new SearchActivatorFactory(serviceProvider);
            var activator = new SearchPropertyActivator(activatorFactory, serviceProvider);

            activator.Activate(configuration, model);
        }

        [Theory]
        [InlineData(nameof(Request.Date), typeof(DateSearch))]
        [InlineData(nameof(Request.Decimal), typeof(DecimalSearch))]
        [InlineData(nameof(Request.Integer), typeof(IntegerSearch))]
        [InlineData(nameof(Request.MultipleDateOption), typeof(MultipleDateOptionSearch))]
        [InlineData(nameof(Request.MultipleIntegerOption), typeof(MultipleIntegerOptionSearch))]
        [InlineData(nameof(Request.MultipleTextOption), typeof(MultipleTextOptionSearch))]
        [InlineData(nameof(Request.OptionalBoolean), typeof(OptionalBooleanSearch))]
        [InlineData(nameof(Request.SingleDateOption), typeof(SingleDateOptionSearch))]
        [InlineData(nameof(Request.SingleIntegerOption), typeof(SingleIntegerOptionSearch))]
        [InlineData(nameof(Request.SingleTextOption), typeof(SingleTextOptionSearch))]
        [InlineData(nameof(Request.Text), typeof(TextSearch))]
        public void Succeeds(string property, Type expected)
        {
            var value = model.GetType().GetProperty(property).GetValue(model);
            value.Should().BeOfType(expected);
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
