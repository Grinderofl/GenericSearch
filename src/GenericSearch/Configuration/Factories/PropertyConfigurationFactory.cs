using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Extensions;

namespace GenericSearch.Configuration.Factories
{
    public class PropertyConfigurationFactory : IPropertyConfigurationFactory
    {
        public PropertyConfiguration Create(PropertyInfo requestProperty, IListDefinition source)
        {
            if (source.PropertyDefinitions.TryGetValue(requestProperty, out var definition))
            {
                return CreateByDefinition(source, definition);
            }

            return CreateByConvention(requestProperty, source);
        }

        private PropertyConfiguration CreateByDefinition(IListDefinition source, IPropertyDefinition definition)
        {
            var requestProperty = definition.RequestProperty;
            Debug.Assert(requestProperty != null, nameof(requestProperty) + " != null");

            var resultProperty = definition.ResultProperty ??
                                 source.ResultProperties.FirstOrDefault(x => x.Name.Equals(definition.RequestProperty.Name));

            var ignored = definition.Ignore.GetValueOrDefault(false);

            if (!ignored &&
                resultProperty != null &&
                !resultProperty.PropertyType.IsAssignableFrom(requestProperty.PropertyType))
            {
                throw new PropertyTypeMismatchException($"The request property {requestProperty.Name} type '{requestProperty.PropertyType.FullName}' is not assignable to '{resultProperty.PropertyType.FullName}'");
            }

            var defaultValue = definition.DefaultValue ?? requestProperty.GetDefaultValue<object>();
            return new PropertyConfiguration(definition.RequestProperty, resultProperty, defaultValue, ignored);
        }

        private PropertyConfiguration CreateByConvention(PropertyInfo requestProperty, IListDefinition source)
        {
            var resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name));
            if (resultProperty == null)
            {
                return null;
            }

            if (!resultProperty.PropertyType.IsAssignableFrom(requestProperty.PropertyType))
            {
                throw new PropertyTypeMismatchException($"The request property {requestProperty.Name} type '{requestProperty.PropertyType.FullName}' is not assignable to '{resultProperty.PropertyType.FullName}'");
            }

            var defaultValue = requestProperty.GetDefaultValue<object>();

            return new PropertyConfiguration(requestProperty, resultProperty, defaultValue, false);
        }
    }
}