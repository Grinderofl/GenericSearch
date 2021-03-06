﻿using System.Reflection;

namespace GenericSearch.Internal.Configuration
{
    public class SortColumnConfiguration : ISortColumnConfiguration
    {
        public SortColumnConfiguration(PropertyInfo requestProperty, PropertyInfo resultProperty, object defaultValue)
        {
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
            DefaultValue = defaultValue;
        }

        public SortColumnConfiguration(string name, object defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
        public string Name { get; }
        public object DefaultValue { get; }
    }
}