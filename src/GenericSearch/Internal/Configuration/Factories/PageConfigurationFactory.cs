using System.Linq;
using System.Reflection;
using GenericSearch.Exceptions;
using GenericSearch.Internal.Definition;
using GenericSearch.Internal.Extensions;
using Microsoft.Extensions.Options;

namespace GenericSearch.Internal.Configuration.Factories
{
    public class PageConfigurationFactory : IPageConfigurationFactory
    {
        private readonly GenericSearchOptions options;
        public PageConfigurationFactory(IOptions<GenericSearchOptions> options)
        {
            this.options = options.Value;
        }

        public PageConfiguration Create(IListDefinition source)
        {
            if (source.PageDefinition != null)
            {
                return CreateByDefinition(source);
            }

            return CreateByConvention(source);
        }

        private PageConfiguration CreateByConvention(IListDefinition source)
        {
            var requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.PagePropertyName));
            if (requestProperty == null)
            {
                return null;
            }

            var resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.PagePropertyName)) ??
                             throw new PropertyNotFoundException($"The page property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");

            var defaultValue = requestProperty.GetDefaultValue<int?>() ?? options.DefaultPage;
            
            return new PageConfiguration(requestProperty, resultProperty, defaultValue);
        }

        private PageConfiguration CreateByDefinition(IListDefinition source)
        {
            PropertyInfo requestProperty;
            PropertyInfo resultProperty;
            if (source.PageDefinition.RequestProperty != null)
            {
                requestProperty = source.PageDefinition.RequestProperty;
                resultProperty = source.PageDefinition.ResultProperty ?? 
                                 source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.PagePropertyName)) ??
                                 throw new PropertyNotFoundException($"The page property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");
                return new PageConfiguration(requestProperty, resultProperty, source.PageDefinition.DefaultValue ?? options.DefaultPage);
            }

            if (!string.IsNullOrWhiteSpace(source.PageDefinition.Name))
            {
                requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(source.PageDefinition.Name));
                if (requestProperty == null)
                {
                    return new PageConfiguration(source.PageDefinition.Name, source.PageDefinition.DefaultValue ?? options.DefaultPage);
                }
            }
            else
            {
                requestProperty = source.RequestProperties.FirstOrDefault(x => x.Name.Equals(options.PagePropertyName));
            }
            
            if (requestProperty == null)
            {
                return new PageConfiguration(options.PagePropertyName, source.PageDefinition.DefaultValue ?? options.DefaultPage);
            }

            resultProperty = source.ResultProperties.FirstOrDefault(x => x.Name.Equals(requestProperty.Name) || x.Name.Equals(options.PagePropertyName)) ??
                             throw new PropertyNotFoundException($"The page property on '{source.ResultType.Name}' is unspecified and no property named '{requestProperty.Name}' has been defined.");
            var defaultValue = source.PageDefinition.DefaultValue ??
                               requestProperty.GetDefaultValue<int?>() ?? 
                               options.DefaultPage;

            return new PageConfiguration(requestProperty, resultProperty, defaultValue);
        }

    }
}