using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Internal.Definition;

namespace GenericSearch.Internal.Configuration.Factories
{
    public class ListConfigurationFactory : IListConfigurationFactory
    {
        private readonly ISearchConfigurationFactory searchConfigurationFactory;
        private readonly IPageConfigurationFactory pageConfigurationFactory;
        private readonly IRowsConfigurationFactory rowsConfigurationFactory;
        private readonly ISortColumnConfigurationFactory sortColumnConfigurationFactory;
        private readonly ISortDirectionConfigurationFactory sortDirectionConfigurationFactory;
        private readonly IPropertyConfigurationFactory propertyConfigurationFactory;
        private readonly IPostRedirectGetConfigurationFactory postRedirectGetConfigurationFactory;
        private readonly ITransferValuesConfigurationFactory transferValuesConfigurationFactory;
        private readonly IModelActivatorConfigurationFactory modelActivatorConfigurationFactory;

        public ListConfigurationFactory(ISearchConfigurationFactory searchConfigurationFactory, 
                                        IPageConfigurationFactory pageConfigurationFactory, 
                                        IRowsConfigurationFactory rowsConfigurationFactory, 
                                        ISortColumnConfigurationFactory sortColumnConfigurationFactory,
                                        ISortDirectionConfigurationFactory sortDirectionConfigurationFactory,
                                        IPropertyConfigurationFactory propertyConfigurationFactory,
                                        IPostRedirectGetConfigurationFactory postRedirectGetConfigurationFactory,
                                        ITransferValuesConfigurationFactory transferValuesConfigurationFactory, 
                                        IModelActivatorConfigurationFactory modelActivatorConfigurationFactory)
        {
            this.searchConfigurationFactory = searchConfigurationFactory;
            this.pageConfigurationFactory = pageConfigurationFactory;
            this.rowsConfigurationFactory = rowsConfigurationFactory;
            this.sortColumnConfigurationFactory = sortColumnConfigurationFactory;
            this.sortDirectionConfigurationFactory = sortDirectionConfigurationFactory;
            this.propertyConfigurationFactory = propertyConfigurationFactory;
            this.postRedirectGetConfigurationFactory = postRedirectGetConfigurationFactory;
            this.transferValuesConfigurationFactory = transferValuesConfigurationFactory;
            this.modelActivatorConfigurationFactory = modelActivatorConfigurationFactory;
        }

        public IListConfiguration Create(IListDefinition source)
        {
            var configuration = new ListConfiguration(source.RequestType, source.ItemType, source.ResultType);
            
            var completedProperties = new List<PropertyInfo>();
            
            foreach (var property in source.RequestProperties)
            {
                if (!IsSearchProperty(property))
                {
                    continue;
                }

                var searchConfiguration = searchConfigurationFactory.Create(property, source);
                configuration.SearchConfigurations.Add(searchConfiguration);
                completedProperties.Add(property);
            }

            configuration.PageConfiguration = pageConfigurationFactory.Create(source);
            if (configuration.PageConfiguration?.RequestProperty != null)
            {
                completedProperties.Add(configuration.PageConfiguration.RequestProperty);
            }

            configuration.RowsConfiguration = rowsConfigurationFactory.Create(source);
            if (configuration.RowsConfiguration?.RequestProperty != null)
            {
                completedProperties.Add(configuration.RowsConfiguration.RequestProperty);
            }

            configuration.SortColumnConfiguration = sortColumnConfigurationFactory.Create(source);
            if (configuration.SortColumnConfiguration?.RequestProperty != null)
            {
                completedProperties.Add(configuration.SortColumnConfiguration.RequestProperty);
            }

            configuration.SortDirectionConfiguration = sortDirectionConfigurationFactory.Create(source);
            if (configuration.SortDirectionConfiguration?.RequestProperty != null)
            {
                completedProperties.Add(configuration.SortDirectionConfiguration.RequestProperty);
            }

            foreach (var requestProperty in source.RequestProperties.Where(x => !completedProperties.Contains(x)))
            {
                var propertyConfiguration = propertyConfigurationFactory.Create(requestProperty, source);
                if (propertyConfiguration == null)
                {
                    continue;
                }

                configuration.PropertyConfigurations.Add(propertyConfiguration);
            }

            configuration.PostRedirectGetConfiguration = postRedirectGetConfigurationFactory.Create(source);
            configuration.TransferValuesConfiguration = transferValuesConfigurationFactory.Create(source);
            configuration.ModelActivatorConfiguration = modelActivatorConfigurationFactory.Create(source);

            return configuration;
        }

        private static bool IsSearchProperty(PropertyInfo property) => property.PropertyType.GetInterfaces().Contains(typeof(ISearch));
    }
}