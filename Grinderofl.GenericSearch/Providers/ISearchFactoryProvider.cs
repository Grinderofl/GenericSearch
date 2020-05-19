using System;
using System.Reflection;
using Grinderofl.GenericSearch.Searches;

namespace Grinderofl.GenericSearch.Providers
{
    /// <summary>
    /// Provides methods to set <see cref="ISearch"/> type property values using a factory method.
    /// </summary>
    public interface ISearchFactoryProvider
    {
        /// <summary>
        /// Provides <see cref="Func{ISearch}"/> values to assign to properties of type <see cref="ISearch"/>
        /// </summary>
        /// <param name="searchProperty">Property to assign the function value to</param>
        /// <returns><see cref="Func{ISearch}"/></returns>
        Func<ISearch> Provide(PropertyInfo searchProperty);
    }
}