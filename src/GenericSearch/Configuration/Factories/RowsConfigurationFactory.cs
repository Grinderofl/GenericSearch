using System.Linq;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Extensions;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration.Factories
{
    public class RowsConfigurationFactory : IRowsConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        public RowsConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        public RowsConfiguration Create(IListDefinition source)
        {
            if (source.RowsDefinition != null)
            {
                return CreateByDefinition(source);
            }

            return CreateByConvention(source);
        }

        private RowsConfiguration CreateByConvention(IListDefinition source)
        {
            var requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.RowsPropertyName));
            if (requestProperty == null)
            {
                return null;
            }

            var resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.RowsPropertyName)) ??
                                 throw new PropertyNotFoundException($"The rows property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");

            var defaultValue = requestProperty.GetDefaultValue<int?>() ?? options.DefaultRows;

            return new RowsConfiguration(requestProperty, resultProperty, defaultValue);
        }

        private RowsConfiguration CreateByDefinition(IListDefinition source)
        {
            PropertyInfo requestProperty;
            PropertyInfo resultProperty;
            if (source.RowsDefinition.RequestProperty != null)
            {
                requestProperty = source.RowsDefinition.RequestProperty;
                resultProperty = source.RowsDefinition.ResultProperty ?? 
                                 source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.RowsPropertyName)) ??
                                 throw new PropertyNotFoundException($"The rows property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");
                return new RowsConfiguration(requestProperty, resultProperty, source.RowsDefinition.DefaultValue ?? options.DefaultRows);
            }

            if (!string.IsNullOrWhiteSpace(source.RowsDefinition.Name))
            {
                requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(source.RowsDefinition.Name));
                if (requestProperty == null)
                {
                    return new RowsConfiguration(source.RowsDefinition.Name, source.RowsDefinition.DefaultValue ?? options.DefaultRows);
                }
            }
            else
            {
                requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.RowsPropertyName));
            }
            
            if (requestProperty == null)
            {
                return new RowsConfiguration(options.RowsPropertyName, source.RowsDefinition.DefaultValue ?? options.DefaultRows);
            }

            resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.RowsPropertyName)) ??
                             throw new PropertyNotFoundException($"The rows property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");
            var defaultValue = source.RowsDefinition.DefaultValue ??
                               requestProperty.GetDefaultValue<int?>() ?? 
                               options.DefaultRows;

            return new RowsConfiguration(requestProperty, resultProperty, defaultValue);
        }
    }
}