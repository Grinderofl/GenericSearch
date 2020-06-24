﻿using System;
using System.Threading.Tasks;
using GenericSearch.Configuration;
using GenericSearch.Configuration.Internal.Caching;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenericSearch.ModelBinding
{
    /// <summary>
    /// Provides information on Filter Configuration
    /// </summary>
    public class GenericSearchModelBinder : IModelBinder
    {
        private readonly IFilterConfiguration filterConfiguration;
        private readonly IModelBinder fallbackModelBinder;
        private readonly IModelCacheProvider modelCacheProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="GenericSearchModelBinder"/>
        /// </summary>
        /// <param name="filterConfiguration">The Filter Configuration for model type</param>
        /// <param name="fallbackModelBinder">Model binder for binding other properties</param>
        /// <param name="modelCacheProvider"></param>
        public GenericSearchModelBinder(IFilterConfiguration filterConfiguration, IModelBinder fallbackModelBinder, IModelCacheProvider modelCacheProvider)
        {
            this.filterConfiguration = filterConfiguration;
            this.fallbackModelBinder = fallbackModelBinder;
            this.modelCacheProvider = modelCacheProvider;
        }

        
        /// <summary>
        /// Binds search properties defined in the respective filter configuration to
        /// the binding context model and creates an instance of the model if it's null.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.Model == null)
            {
                bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
            }

            foreach (var searchProperty in filterConfiguration.SearchConfigurations)
            {
                searchProperty.RequestProperty.SetValue(bindingContext.Model, searchProperty.SearchFactory());
            }
            
            filterConfiguration.PageConfiguration
                               .RequestRowsProperty?
                               .SetValue(bindingContext.Model, filterConfiguration.PageConfiguration?.DefaultRowsPerPage);

            filterConfiguration.PageConfiguration
                               .RequestPageNumberProperty?
                               .SetValue(bindingContext.Model, filterConfiguration.PageConfiguration?.DefaultPageNumber);

            filterConfiguration.SortConfiguration
                               .RequestSortDirection?
                               .SetValue(bindingContext.Model, filterConfiguration.SortConfiguration?.DefaultSortDirection);

            filterConfiguration.SortConfiguration
                               .RequestSortProperty?
                               .SetValue(bindingContext.Model, filterConfiguration.SortConfiguration?.DefaultSortProperty?.Name);

            var modelCache = modelCacheProvider.Provide();
            modelCache.CacheModel(bindingContext.Model);

            await fallbackModelBinder.BindModelAsync(bindingContext);
        }
    }
}