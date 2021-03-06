﻿using System;
using Microsoft.Extensions.Options;

namespace GenericSearch.UnitTests.Internal.Configuration.Factories
{
    public abstract class ConfigurationFactoryTestBase
    {
        protected IOptions<GenericSearchOptions> Options => CreateOptions();

        protected IOptions<GenericSearchOptions> CreateOptions(Action<GenericSearchOptions> action = null)
        {
            var options = new GenericSearchOptions();
            action?.Invoke(options);
            return new OptionsWrapper<GenericSearchOptions>(options);
        }
    }
}