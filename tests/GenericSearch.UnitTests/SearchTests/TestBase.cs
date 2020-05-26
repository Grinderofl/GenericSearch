using System;
using GenericSearch;
using GenericSearch.Configuration;
using GenericSearch.Providers;
using GenericSearch.UnitTests;
using Microsoft.Extensions.Options;
using Moq;

// ReSharper disable All

namespace Grinderofl.GenericSearch.UnitTests.BooleanSearchTests
{
    public abstract class TestBase
    {
        protected IFilterConfigurationProvider Provider { get; }
        
        
        protected TestBase()
        {
            var options = new OptionsWrapper<GenericSearchOptions>(new GenericSearchOptions());
            var conventionOptions = new OptionsWrapper<GenericSearchConventionOptions>(new GenericSearchConventionOptions());

            var profile = CreateProfile();
            var factory = new ConventionFilterConfigurationFactory(new SearchFactoryProvider(), conventionOptions, options);

            Provider = new FilterConfigurationProvider(factory, new []{profile});
        }

        protected virtual TestProfile CreateProfile()
        {
            return new TestProfile();
        }
    }
}
