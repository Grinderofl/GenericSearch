using System.Linq;
using System.Reflection;
using GenericSearch.Definition;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Extensions;
using GenericSearch.Searches;
using Microsoft.Extensions.Options;

namespace GenericSearch.Configuration.Factories
{
    public class SortDirectionConfigurationFactory : ISortDirectionConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        public SortDirectionConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        public SortDirectionConfiguration Create(IListDefinition source)
        {
            if (source.SortDirectionDefinition != null)
            {
                return CreateByDefinition(source);
            }

            return CreateByConvention(source);
        }

        private SortDirectionConfiguration CreateByConvention(IListDefinition source)
        {
            var requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.SortDirectionPropertyName));
            if (requestProperty == null)
            {
                return null;
            }

            var resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.SortDirectionPropertyName)) ??
                                 throw new PropertyNotFoundException($"The sort Direction property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");

            var defaultValue = requestProperty.GetDefaultValue<Direction?>() ?? options.SortDirection;

            return new SortDirectionConfiguration(requestProperty, resultProperty, defaultValue);
        }

        private SortDirectionConfiguration CreateByDefinition(IListDefinition source)
        {
            PropertyInfo requestProperty;
            PropertyInfo resultProperty;
            if (source.SortDirectionDefinition.RequestProperty != null)
            {
                requestProperty = source.SortDirectionDefinition.RequestProperty;
                resultProperty = source.SortDirectionDefinition.ResultProperty ??
                                 source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.SortDirectionPropertyName)) ??
                                 throw new PropertyNotFoundException($"The sort Direction property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");
                return new SortDirectionConfiguration(requestProperty, resultProperty, source.SortDirectionDefinition.DefaultValue ?? options.SortDirection);
            }
            else
            {
                requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.SortDirectionPropertyName));
            }

            if (requestProperty == null)
            {
                return new SortDirectionConfiguration(options.SortDirectionPropertyName, source.SortDirectionDefinition.DefaultValue ?? options.SortDirection);
            }

            resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.SortDirectionPropertyName)) ??
                             throw new PropertyNotFoundException($"The sort Direction property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");

            var defaultValue = source.SortDirectionDefinition.DefaultValue ??
                               requestProperty.GetDefaultValue<Direction?>() ??
                               options.SortDirection;

            return new SortDirectionConfiguration(requestProperty, resultProperty, defaultValue);
        }
    }
}