﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Activation.Finders;
using GenericSearch.Internal.Configuration;
using GenericSearch.Internal.Configuration.Factories;
using GenericSearch.Internal.Definition;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GenericSearch.UnitTests.Internal.Configuration
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ListConfigurationProviderTests
    {
        private readonly ListConfigurationFactory factory;
        private readonly Mock<IOptions<GenericSearchOptions>> optionsMock;

        public ListConfigurationProviderTests()
        {
            var options = new GenericSearchOptions();

            optionsMock = new Mock<IOptions<GenericSearchOptions>>();
            optionsMock.SetupGet(x => x.Value).Returns(options);

            var filterConfigurationFactory = new SearchConfigurationFactory(new PascalCasePropertyPathFinder());
            var pageConfigurationFactory = new PageConfigurationFactory(optionsMock.Object);
            var rowsConfigurationFactory = new RowsConfigurationFactory(optionsMock.Object);
            var sortColumnConfigurationFactory = new SortColumnConfigurationFactory(optionsMock.Object);
            var sortDirectionConfigurationFactory = new SortDirectionConfigurationFactory(optionsMock.Object);
            var propertyConfigurationFactory = new PropertyConfigurationFactory();
            var postRedirectGetConfigurationFactory = new PostRedirectGetConfigurationFactory(optionsMock.Object);
            var transferValuesConfigurationFactory = new TransferValuesConfigurationFactory(optionsMock.Object);
            var requestFactoryConfigurationFactory = new ModelActivatorConfigurationFactory(optionsMock.Object);
            factory = new ListConfigurationFactory(filterConfigurationFactory,
                                                   pageConfigurationFactory,
                                                   rowsConfigurationFactory,
                                                   sortColumnConfigurationFactory,
                                                   sortDirectionConfigurationFactory,
                                                   propertyConfigurationFactory,
                                                   postRedirectGetConfigurationFactory,
                                                   transferValuesConfigurationFactory,
                                                   requestFactoryConfigurationFactory);
        }

        private class ValidProfile : ListProfile
        {
            public ValidProfile()
            {
                AddList<Request, Item, Result>();
            }
        }

        private class DuplicateProfile : ListProfile
        {
            public DuplicateProfile()
            {
                AddList<Request, Item, Result>();
            }
        }

        [Fact]
        public void Construct_Valid_Definition_Suceeds()
        {
            var source = new List<IListDefinitionSource> {new ValidProfile()};
            var provider = new ListConfigurationProvider(source, factory, optionsMock.Object);
            provider.Configurations.Should().HaveCount(1);
            provider.Configurations.First().Value.Should().NotBeNull();
            provider.Configurations.First().Value.ResultType.Name.Should().Be(nameof(Result));
        }
        
        [Fact]
        public void Construct_Duplicate_Definition_Throws()
        {
            var source = new List<IListDefinitionSource> {new ValidProfile(), new DuplicateProfile()};
            Action action = () => new ListConfigurationProvider(source, factory, optionsMock.Object);
            action.Should().ThrowExactly<InvalidFilterConfigurationException>();
        }


        [Fact]
        public void GetConfiguration_ValidType_Succeeds()
        {
            var source = new List<IListDefinitionSource> {new ValidProfile()};
            var provider = new ListConfigurationProvider(source, factory, optionsMock.Object);

            var result = provider.GetConfiguration(typeof(Request));

            result.Should().NotBeNull();
            result.ResultType.Name.Should().Be(nameof(Result));
        }

        [Fact]
        public void GetConfiguration_InvalidType_ReturnsNull()
        {
            var source = new List<IListDefinitionSource> {new ValidProfile()};
            var provider = new ListConfigurationProvider(source, factory, optionsMock.Object);

            var result = provider.GetConfiguration(typeof(Result));

            result.Should().BeNull();
        }

        private class Item
        {
        }

        private class Request
        {
        }

        private class Result
        {
        }
    }
}
