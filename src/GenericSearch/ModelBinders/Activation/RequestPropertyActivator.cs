using System.Collections.Generic;
using System.Linq;
using GenericSearch.Configuration;
using GenericSearch.Searches.Activation.Factories;

namespace GenericSearch.ModelBinders.Activation
{
    public class RequestPropertyActivator : IRequestPropertyActivator
    {
        private readonly ISearchActivatorFactory searchActivatorFactory;

        public RequestPropertyActivator(ISearchActivatorFactory searchActivatorFactory)
        {
            this.searchActivatorFactory = searchActivatorFactory;
        }

        public void Activate(ListConfiguration configuration, object model)
        {
            ActivateSearchProperties(configuration.SearchConfigurations, model);
            ActivateSortDirection(configuration.SortDirectionConfiguration, model);
            ActivateSortColumn(configuration.SortColumnConfiguration, model);
            ActivatePage(configuration.PageConfiguration, model);
            ActivateRows(configuration.RowsConfiguration, model);
            ActivateProperties(configuration.PropertyConfigurations, model);
        }

        private void ActivateSearchProperties(IEnumerable<SearchConfiguration> configurations, object model)
        {
            foreach (var configuration in configurations)
            {
                ActivateSearchProperty(configuration, model);
            }
        }

        private void ActivateSearchProperty(SearchConfiguration configuration, object model)
        {
            var activator = searchActivatorFactory.Create(configuration.RequestProperty.PropertyType);
            var value = activator.Create(configuration.RequestProperty);

            configuration.RequestProperty.SetValue(model, value);
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