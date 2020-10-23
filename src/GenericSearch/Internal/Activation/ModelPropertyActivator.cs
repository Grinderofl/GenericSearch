using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericSearch.Internal.Activation.Factories;
using GenericSearch.Internal.Configuration;

namespace GenericSearch.Internal.Activation
{
    public class ModelPropertyActivator : IModelPropertyActivator
    {
        private readonly ISearchActivatorFactory searchActivatorFactory;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Creates a new instance of <see cref="ModelPropertyActivator"/>.
        /// </summary>
        /// <param name="searchActivatorFactory"></param>
        /// <param name="serviceProvider"></param>
        public ModelPropertyActivator(ISearchActivatorFactory searchActivatorFactory, IServiceProvider serviceProvider)
        {
            this.searchActivatorFactory = searchActivatorFactory;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Attempts to activate all relevant search, sort, pagination, and other specified
        /// properties of the <paramref name="model"/>.
        /// </summary>
        /// <param name="configuration">Relevant <see cref="IListConfiguration"/> for the <paramref name="model"/> type.</param>
        /// <param name="model">Instance of the model type.</param>
        /// <exception cref="ArgumentNullException">Provided <paramref name="configuration"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Provided <paramref name="model"/> is null.</exception>
        public void Activate(IListConfiguration configuration, object model)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Provided configuration is null");
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Provided model is null.");
            }

            ActivateSearchProperties(configuration, model);
            ActivateSortDirection(configuration.SortDirectionConfiguration, model);
            ActivateSortColumn(configuration.SortColumnConfiguration, model);
            ActivatePage(configuration.PageConfiguration, model);
            ActivateRows(configuration.RowsConfiguration, model);
            ActivateProperties(configuration.PropertyConfigurations, model);
        }
        
        /// <summary>
        /// Activates <see cref="ISearch"/> type properties on the provided <paramref name="model"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="model"></param>
        protected virtual void ActivateSearchProperties(IListConfiguration configuration, object model)
        {
            foreach (var searchConfiguration in configuration.SearchConfigurations)
            {
                var value = ActivateSearchProperty(searchConfiguration);
                searchConfiguration.RequestProperty.SetValue(model, value);
            }
        }

        /// <summary>
        /// Creates an instance of type <see cref="PropertyInfo.PropertyType"/> specified in <see cref="ISearchConfiguration.RequestProperty"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        protected virtual ISearch ActivateSearchProperty(ISearchConfiguration configuration)
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

        /// <summary>
        /// Activates the sort direction property as specified in <see cref="ISortDirectionConfiguration.RequestProperty"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="model"></param>
        protected virtual void ActivateSortDirection(ISortDirectionConfiguration configuration, object model) =>
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        /// <summary>
        /// Activates the sort column property as specified in <see cref="ISortColumnConfiguration.RequestProperty"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="model"></param>
        protected virtual void ActivateSortColumn(ISortColumnConfiguration configuration, object model) =>
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        /// <summary>
        /// Activates the page number property as specified in <see cref="IPageConfiguration.RequestProperty"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="model"></param>
        protected virtual void ActivatePage(IPageConfiguration configuration, object model) => 
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        /// <summary>
        /// Activates the row count property as specified in <see cref="IRowsConfiguration.RequestProperty"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="model"></param>
        protected virtual void ActivateRows(IRowsConfiguration configuration, object model) => 
            configuration?.RequestProperty?.SetValue(model, configuration.DefaultValue);

        /// <summary>
        /// Activates all non-genericsearch-relevant properties which have been assigned a default value.
        /// </summary>
        /// <param name="configurations"></param>
        /// <param name="model"></param>
        protected virtual void ActivateProperties(List<IPropertyConfiguration> configurations, object model)
        {
            foreach (var configuration in configurations.Where(x => !x.Ignored && x.DefaultValue != null))
            {
                configuration.RequestProperty.SetValue(model, configuration.DefaultValue);
            }
        }
    }
}