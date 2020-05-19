using System.Collections.Generic;
using System.Text;
using Grinderofl.GenericSearch.Configuration;

namespace Grinderofl.GenericSearch
{

    /// <summary>
    ///     Provides a configuration source for Generic Searchs.
    /// </summary>
    public class GenericSearchProfile : IGenericSearchProfile
    {

        /// <summary>
        /// Creates a filter expression to search, sort, and paginate a query of <typeparamref name="TItem"/>
        /// using criteria from <typeparamref name="TRequest"/>, transfer the filters to <typeparamref name="TResult"/>,
        /// and provide more configuration options.
        /// </summary>
        /// <typeparam name="TItem">Entity/Projection queryable type</typeparam>
        /// <typeparam name="TRequest">Request/Parameter type</typeparam>
        /// <typeparam name="TResult">Result/ViewModel type</typeparam>
        /// <returns>Filter expression for more configuration options</returns>
        public IFilterExpression<TItem, TRequest, TResult> CreateFilter<TItem, TRequest, TResult>()
        {
            var filterExpression = new FilterExpression<TItem, TRequest, TResult>();
            FilterConfigurations.Add(filterExpression);
            return filterExpression;
        }

        internal IList<IFilterConfiguration> FilterConfigurations = new List<IFilterConfiguration>();

        IList<IFilterConfiguration> IGenericSearchProfile.FilterConfigurations => FilterConfigurations;
    }

    
}
