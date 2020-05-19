using System;
using Grinderofl.GenericSearch.Configuration;
using Grinderofl.GenericSearch.Exceptions;

namespace Grinderofl.GenericSearch.Providers
{
    /// <summary>
    /// Provides Filter Configurations for searching and model binding
    /// </summary>
    public interface IFilterConfigurationProvider
    {
        /// <summary>
        /// Provides a <see cref="IFilterConfiguration"/> for given request/parameter model type.
        /// </summary>
        /// <param name="modelType">Request/Parameter model type</param>
        /// <exception cref="InvalidFilterConfigurationException">More than one filter configuration was found for given model type</exception>
        /// <returns>Instance of <see cref="IFilterConfiguration"/></returns>
        IFilterConfiguration Provide(Type modelType);
    }
}