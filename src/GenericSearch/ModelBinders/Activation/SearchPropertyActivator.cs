using System;
using System.Collections.Generic;
using System.Linq;
using GenericSearch.Configuration;
using GenericSearch.Searches;
using GenericSearch.Searches.Activation.Factories;

namespace GenericSearch.ModelBinders.Activation
{
    public class SearchPropertyActivator : ISearchPropertyActivator
    {
        private readonly ISearchActivatorFactory searchActivatorFactory;
        private readonly IServiceProvider serviceProvider;

        public SearchPropertyActivator(ISearchActivatorFactory searchActivatorFactory, IServiceProvider serviceProvider)
        {
            this.searchActivatorFactory = searchActivatorFactory;
            this.serviceProvider = serviceProvider;
        }

        public void Activate(ListConfiguration configuration, object model)
        {
            ActivateSearchProperties(configuration, model);
            ActivateSortDirection(configuration.SortDirectionConfiguration, model);
            ActivateSortColumn(configuration.SortColumnConfiguration, model);
            ActivatePage(configuration.PageConfiguration, model);
            ActivateRows(configuration.RowsConfiguration, model);
            ActivateProperties(configuration.PropertyConfigurations, model);
        }

        private void ActivateSearchProperties(ListConfiguration configuration, object model)
        {
            foreach (var searchConfiguration in configuration.SearchConfigurations)
            {
                var value = ActivateSearchProperty(searchConfiguration);
                searchConfiguration.RequestProperty.SetValue(model, value);
            }
        }

        private ISearch ActivateSearchProperty(SearchConfiguration configuration)
        {
            if (configuration.Constructor != null)
            {
                return configuration.Constructor();
            }

            if (configuration.Activator != null)
            {
                return configuration.Activator(serviceProvider).Activate(configuration.ItemPropertyPath);
            }

            var activator = searchActivatorFactory.Create(configuration.RequestProperty.PropertyType);
            return activator.Activate(configuration.ItemPropertyPath);
        }

        private void ActivateSortDirection(SortDirectionConfiguration configuration, object model) =>
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        private void ActivateSortColumn(SortColumnConfiguration configuration, object model) =>
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        private void ActivatePage(PageConfiguration configuration, object model) => 
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        private void ActivateRows(RowsConfiguration configuration, object model) => 
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        private void ActivateProperties(IEnumerable<PropertyConfiguration> configurations, object model)
        {
            foreach (var configuration in configurations.Where(x => !x.Ignored && x.DefaultValue != null))
            {
                configuration.RequestProperty.SetValue(model, configuration.DefaultValue);
            }
        }
    }
}