﻿using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IListConfigurationFactory
    {
        ListConfiguration Create(IListDefinition source);
    }
}