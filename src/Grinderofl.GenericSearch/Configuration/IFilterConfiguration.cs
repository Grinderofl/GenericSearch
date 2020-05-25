using System;
using System.Collections.Generic;

namespace Grinderofl.GenericSearch.Configuration
{
    /// <summary>
    /// Contains information of filter criteria expression
    /// </summary>
    public interface IFilterConfiguration
    {
        /// <summary>
        /// Queryable type
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// Request/Parameter type
        /// </summary>
        Type RequestType { get; }

        /// <summary>
        /// Result/ViewModel type
        /// </summary>
        Type ResultType { get; }

        /// <summary>
        /// Configurations of the criteria on the filter
        /// </summary>
        IList<ISearchConfiguration> SearchConfigurations { get; }

        /// <summary>
        /// Configuration of sorting
        /// </summary>
        ISortConfiguration SortConfiguration { get; }

        /// <summary>
        /// Configuration of paging
        /// </summary>
        IPageConfiguration PageConfiguration { get; }

        /// <summary>
        /// Configuration of post to get redirection
        /// </summary>
        IRedirectPostToGetConfiguration RedirectPostToGetConfiguration { get; }

        /// <summary>
        /// Configuration of copying request filter values to result filters
        /// </summary>
        ICopyRequestFilterValuesConfiguration CopyRequestFilterValuesConfiguration { get; }

        /// <summary>
        /// Specifies the name of Controller Actions GenericSearch should perform Post to Get redirects and
        /// Request/Parameter to Result/ViewModel copying against.
        /// </summary>
        string ListActionName { get; }

    }
}