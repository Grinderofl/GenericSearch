﻿using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IListConfigurationFactory
    {
        IListConfiguration Create(IListDefinition source);
    }
}