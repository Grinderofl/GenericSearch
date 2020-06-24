﻿using System.Reflection;
using GenericSearch.Definition;

namespace GenericSearch.Configuration.Factories
{
    public interface IPropertyConfigurationFactory
    {
        PropertyConfiguration Create(PropertyInfo requestProperty, IListDefinition source);
    }
}