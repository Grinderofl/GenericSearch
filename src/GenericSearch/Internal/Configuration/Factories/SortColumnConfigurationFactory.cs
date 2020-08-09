using System.Linq;
using System.Reflection;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Definition;
using GenericSearch.Internal.Extensions;
using Microsoft.Extensions.Options;

namespace GenericSearch.Internal.Configuration.Factories
{
    public class SortColumnConfigurationFactory : ISortColumnConfigurationFactory
    {
        private readonly GenericSearchOptions options;

        public SortColumnConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        public SortColumnConfiguration Create(IListDefinition source)
        {
            if (source.SortColumnDefinition != null)
            {
                return CreateByDefinition(source);
            }

            return CreateByConvention(source);
        }

        private SortColumnConfiguration CreateByConvention(IListDefinition source)
        {
            var requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.SortColumnPropertyName));
            if (requestProperty == null)
            {
                return null;
            }

            var resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.SortColumnPropertyName)) ??
                                 throw new PropertyNotFoundException($"The sort column property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");

            var defaultValue = requestProperty.GetDefaultValue<object>();

            return new SortColumnConfiguration(requestProperty, resultProperty, defaultValue);
        }

        private SortColumnConfiguration CreateByDefinition(IListDefinition source)
        {
            PropertyInfo requestProperty;
            PropertyInfo resultProperty;
            if (source.SortColumnDefinition.RequestProperty != null)
            {
                requestProperty = source.SortColumnDefinition.RequestProperty;
                resultProperty = source.SortColumnDefinition.ResultProperty ??
                                 source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.SortColumnPropertyName)) ??
                                 throw new PropertyNotFoundException($"The sort column property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");
                return new SortColumnConfiguration(requestProperty, resultProperty, source.SortColumnDefinition.DefaultValue);
            }
            else
            {
                requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.SortColumnPropertyName));
            }

            if (requestProperty == null)
            {
                return new SortColumnConfiguration(options.SortColumnPropertyName, source.SortColumnDefinition.DefaultValue);
            }

            resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.SortColumnPropertyName)) ??
                             throw new PropertyNotFoundException($"The sort column property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");

            var defaultValue = source.SortColumnDefinition.DefaultValue ??
                               requestProperty.GetDefaultValue<object>();

            return new SortColumnConfiguration(requestProperty, resultProperty, defaultValue);
        }
    }
}