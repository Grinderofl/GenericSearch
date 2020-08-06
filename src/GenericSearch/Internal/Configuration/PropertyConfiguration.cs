using System.Diagnostics;
using System.Reflection;

namespace GenericSearch.Internal.Configuration
{
    [DebuggerDisplay("PropertyConfiguration: Request = {RequestProperty.Name} Result = {ResultProperty.Name} DefaultValue = {DefaultValue}")]
    public class PropertyConfiguration : IPropertyConfiguration
    {
        public PropertyConfiguration(PropertyInfo requestProperty, PropertyInfo resultProperty, object defaultValue, bool ignored)
        {
            RequestProperty = requestProperty;
            ResultProperty = resultProperty;
            DefaultValue = defaultValue;
            Ignored = ignored;
        }

        public PropertyInfo RequestProperty { get; }
        public PropertyInfo ResultProperty { get; }
        public object DefaultValue { get; }
        public bool Ignored { get; }
    }
}