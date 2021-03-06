﻿using System.Reflection;

namespace GenericSearch.Internal.Configuration
{
    public interface IPageConfiguration
    {
        PropertyInfo RequestProperty { get; }
        PropertyInfo ResultProperty { get; }
        int DefaultValue { get; }
        string Name { get; }
    }
}