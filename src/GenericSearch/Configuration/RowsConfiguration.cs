﻿using System.Reflection;

namespace GenericSearch.Configuration
{
    public class RowsConfiguration
    {
        public RowsConfiguration(PropertyInfo requestProperty, PropertyInfo resultProperty, int defaultValue)
        {
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
            DefaultValue = defaultValue;
        }

        public RowsConfiguration(string name, int defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
        public string Name { get; }
        public int DefaultValue { get; }
    }
}